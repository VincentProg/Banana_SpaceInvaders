using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpaceShipGenerator : MonoBehaviour
{
    [SerializeField] private List<Mesh> mSpaceShipMesh;
    [SerializeField] private List<Texture> mPossibleColors;
    [SerializeField] private Material mSpaceShipMaterial;

    private void Awake()
    {
        GameObject spaceShip = new GameObject("SpaceShip Visual");
        spaceShip.transform.SetParent(transform);
        spaceShip.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        spaceShip.transform.localPosition = Vector3.zero;
        MeshFilter meshFilter = spaceShip.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = spaceShip.AddComponent<MeshRenderer>();
        meshFilter.mesh = mSpaceShipMesh[Random.Range(0, mSpaceShipMesh.Count)];
        mSpaceShipMaterial.mainTexture = mPossibleColors[Random.Range(0, mPossibleColors.Count)];
        meshRenderer.material = mSpaceShipMaterial;
    }
}
