using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    [SerializeField] private List<Cube> _cubes;

    private void OnEnable()
    {
        foreach (var cube in _cubes)
        {
            cube.Dividing += Create;
            cube.Removing += DeleteCube;
        }
    }

    private void OnDisable()
    {
        foreach (var cube in _cubes)
        {
            cube.Dividing -= Create;
            cube.Removing -= DeleteCube;
        }
    }

    private void DeleteCube(Cube cube)
    {
        _cubes.Remove(cube);
        cube.Removing -= DeleteCube;
        cube.Dividing -= Create;
    }

    private void Create(Cube explodedCube, int cubesCount)
    {
        for (int i = 0; i < cubesCount; i++)
        {
            Cube cube = Instantiate(explodedCube, explodedCube.transform.position, Quaternion.identity);
            _cubes.Add(cube);
            cube.Dividing += Create;
            cube.Removing += DeleteCube;
            cube.Init(explodedCube.ChanceCreate, cube.ExplosionForce, cube.ExplosionRadius);
        }
    }
}
