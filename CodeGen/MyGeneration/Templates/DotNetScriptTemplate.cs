// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using Zeus;
using Zeus.Data;
using Zeus.DotNetScript;
using Zeus.UserInterface;
using MyMeta;
using Dnp.Utils;

/// <summary>
/// This file is only present to facilitate building the project in Visual Studio.
/// This code is automatically added by MyGeneration templating engine.
/// </summary>
public abstract class DotNetScriptTemplate : _DotNetScriptTemplate
{
	protected Zeus.UserInterface.GuiController ui;
	protected MyMeta.dbRoot MyMeta;
	protected Dnp.Utils.Utils DnpUtils;

	public DotNetScriptTemplate(IZeusContext context) : base(context)
	{
		this.ui = context.Objects["ui"] as Zeus.UserInterface.GuiController;
		this.MyMeta = context.Objects["MyMeta"] as MyMeta.dbRoot;
		this.DnpUtils = context.Objects["DnpUtils"] as Dnp.Utils.Utils;
	}
}
