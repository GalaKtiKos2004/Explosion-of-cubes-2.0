using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private const int MaxChanceCreate = 100;

    [SerializeField] private Exploder _exploder;
    [SerializeField] private int _minCreate = 2;
    [SerializeField] private int _maxCreate = 6;
    [SerializeField] private int _chanceDivider = 2;
    [SerializeField] private int _scaleDivider = 2;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    private int _chanceCreate = 100;

    public event Action<Cube, int> Dividing;
    public event Action<Cube> Removing;

    public Rigidbody Rigidbody { get; private set; }
    
    public float ExplosionForce => _explosionForce;
    public float ExplosionRadius => _explosionRadius;

    public int ChanceCreate => _chanceCreate;

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Init(int chanceCreate)
    {
        transform.localScale /= _scaleDivider;
        _chanceCreate = chanceCreate / _chanceDivider;
        _explosionForce *= _scaleDivider;
        _explosionRadius *= _scaleDivider;
    }

    private void Explode()
    {
        if (CanDivide())
        {
            int cubesCount = UnityEngine.Random.Range(_minCreate, _maxCreate + 1);

            Dividing?.Invoke(this, cubesCount);
        }
        else
        {
            List<Rigidbody> explodableObjects = GetExolodableObjects();

            foreach (var explodableObject in explodableObjects)
                _exploder.Explode(explodableObject, transform.position, ExplosionForce, ExplosionRadius);
        }

        Removing?.Invoke(this);
    }

    private List<Rigidbody> GetExolodableObjects()
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
        return UnityEngine.Random.Range(0, MaxChanceCreate + 1) < _chanceCreate;
    }
}
