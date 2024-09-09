using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    [SerializeField] private Exploder _exploder;
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
    }

    private void Create(Cube explodedCube)
    {
        Cube cube = Instantiate(explodedCube, explodedCube.transform.position, Quaternion.identity);
        _cubes.Add(cube);
        cube.Dividing += Create;
        cube.Removing += DeleteCube;
        cube.Init();

        Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();
        if (cubeRigidbody != null)
            _exploder.Explode(cubeRigidbody, cubeRigidbody.transform.position, explodedCube.ExplosionForce, explodedCube.ExplosionRadius);
    }
}
