using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public float ExplosionForce;
    public float ExplosionRadius;

    [SerializeField] private Exploder _exploder;
    [SerializeField] private int _minCreate = 2;
    [SerializeField] private int _maxCreate = 6;
    [SerializeField] private int _chanceDivider = 2;
    [SerializeField] private int _scaleDivider = 2;

    private int _maxChanceCreate = 100;
    private int _chanceCreate = 100;

    public event Action<Cube> Dividing;
    public event Action<Cube> Removing;

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void OnMouseDown()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Init()
    {
        transform.localScale /= _scaleDivider;
        _chanceCreate /= _chanceDivider;
        ExplosionForce *= _scaleDivider;
        ExplosionRadius *= _scaleDivider;
    }

    private void Explode()
    {
        if (CanDivide())
        {
            int cubeNumbers = UnityEngine.Random.Range(_minCreate, _maxCreate + 1);

            for (int i = 0; i < cubeNumbers; i++)
            {
                Dividing?.Invoke(this);
            }
        }
        else
        {
            List<Rigidbody> explodableCubes = GetExolodableOblects();

            foreach (var cube in explodableCubes)
                _exploder.Explode(cube.GetComponent<Rigidbody>(), transform.position, ExplosionForce, ExplosionRadius);
        }

        Removing?.Invoke(this);
    }

    private List<Rigidbody> GetExolodableOblects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);

        List<Rigidbody> cubes = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                cubes.Add(hit.attachedRigidbody);

        return cubes;
    }

    private bool CanDivide()
    {
        return UnityEngine.Random.Range(0, _maxChanceCreate + 1) < _chanceCreate;
    }
}
