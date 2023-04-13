using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class GuideDB : ScriptableObject
{
	public List<GuideDBEntity> Entities; // Replace 'EntityType' to an actual type that is serializable.
}
