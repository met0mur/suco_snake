using System.Collections;
using System.Collections.Generic;
using SucoSnake.Core;
using UnityEngine;

public class Initializer : MonoBehaviour
{
	private ControllerAggregator _controllers;

	// Use this for initialization
	void Start ()
	{
		_controllers = new ControllerAggregator();
		_controllers.Add( new TestInitialierController() );
		_controllers.Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_controllers.Update();
	}
}

public class TestInitialierController : Controller
{
	protected override void InternalInit()
	{
		base.InternalInit();

		var factory = new CellContentFactory();

		var cell = new Cell();
		cell.Init( factory );

	}
}
