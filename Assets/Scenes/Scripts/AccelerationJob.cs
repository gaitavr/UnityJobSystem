using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct AccelerationJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Vector3> Positions;
    [ReadOnly]
    public NativeArray<Vector3> Velocities;

    public NativeArray<Vector3> Accelerations;

    public float DestinationThreshold;
    public Vector3 Weights;

    private int Count => Positions.Length - 1;
    
    public void Execute(int index)
    {
        Vector3 averageSpread = Vector3.zero,
            averageVelocity = Vector3.zero,
            averagePosition = Vector3.zero;

        for (int i = 0; i < Count; i++)
        {
            if(i == index)
                continue;
            var targetPos = Positions[i];
            var posDifference = Positions[index] - targetPos;
            if(posDifference.magnitude > DestinationThreshold)
                continue;
            averageSpread += posDifference.normalized;
            averageVelocity += Velocities[i];
            averagePosition += targetPos;
        }

        Accelerations[index] += (averageSpread / Count) * Weights.x + 
            (averageVelocity / Count) * Weights.y + 
            (averagePosition / Count - Positions[index]) * Weights.z;
    }
}