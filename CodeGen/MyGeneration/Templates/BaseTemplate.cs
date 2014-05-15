// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

using System.Text.RegularExpressions;
using System.Globalization;

//#region conditional using directives
////The following using directives get automatically added by the templating engine
////and need to be commented out for code generation. They are, however, required to
////compile this file to check for compilation error prior to code generation.
//using System;
//using System.Collections;
//using Zeus;
//using Zeus.Data;
//using Zeus.DotNetScript;
//using Zeus.UserInterface;
//using MyMeta;
//using Dnp.Utils;
//#endregion

public abstract class BaseTemplate : DotNetScriptTemplate
{
	#region constants
	// user map related constants
	protected static readonly string USER_MAP_FILENAME = "NHibernateMappings.xml";
	protected static readonly string USER_MAP_CSHARP_TO_NHIBERNATE_SUPPORTED_TYPE = "C# to NHibernateSupportedType";
	protected static readonly string USER_MAP_CSHARP_TO_NULLABLE_TYPE = "C# to NullableType";
	protected static readonly string USER_MAP_CSHARP_TO_NHIBERNATE_MAPPING_TYPE = "C# to NHibernateMappingType";
	// enumeration tables
	protected static readonly string SITE_BASE_DATA_FILE_NAME = "SiteBaseData.xml";
	// CP = Custom MyMeta Metadata Property
	protected static readonly string CP_IGNORE = "Ignore";
	protected static readonly string CP_DATA_TYPE = "DataType";
	protected static readonly string CP_NULLABLE = "Nullable";
	protected static readonly string CP_ENUM = "Enum";
	protected static readonly string CP_LOGICAL_GROUP = "LogicalGroup";
	protected static readonly string CP_LAZY = "Lazy";
	protected static readonly string CP_FETCH_ALL = "FetchAll";
	protected static readonly string CP_FINDER_COLUMN = "FinderColumn";
	protected static readonly string CP_PARENT_COLUMN = "ParentColumn";
	protected static readonly string CP_CLASS_REF_FOR_FK = "ClassRefForFk";
	protected static readonly string CP_TABLE_REF = "TableRef";
	protected static readonly string CP_COLLECTION_PREFIX = "Collection";
	protected static readonly string CP_NAME_COLUMN = "NameColumn";
	protected static readonly string CP_CODE_COLUMN = "CodeColumn";
	protected static readonly string CP_ENUM_COLUMN = "EnumColumn";
	protected static readonly string CP_NAME = "Name";
	protected static readonly string CP_ENTITY_PREFIX = "EntityPrefix";
	protected static readonly string CP_CASCADE = "Cascade";
	protected static readonly string CP_ORDER_BY = "OrderBy";
	protected static readonly string CP_KEY_COLUMN = "KeyColumn";
	protected static readonly string CP_MAPPING_TYPE = "MappingType";
	protected static readonly string CP_DTO_TYPE = "DtoType";
	protected static readonly string CP_ITEM_ENUMERATION = "ItemEnumeration";
	protected static readonly string CP_ITEM_TABLE_REF = "ItemTableRef";
	protected static readonly string CP_ITEM_KEY_COLUMN = "ItemKeyColumn";
	// CP value constants
	protected static readonly string NAME_COLUMN_VAL_NONE = "None";
	protected static readonly string CODE_COLUMN_VAL_NONE = "None";
	protected static readonly string FINDER_COLUMN_VAL_COLLECTION = "Collection";
	protected static readonly string FINDER_COLUMN_VAL_UNIQUE_RESULT = "Unique";
	// other constants
	protected static readonly string HEADER_TXT_FILE = @"..\Templates\header.txt";
	protected static readonly int MAX_COLLECTIONS = 10;
	protected static readonly string INTERFACE_PREFIX = "I";
	protected static readonly string DAO_SUFFIX = "Dao";
	protected static readonly string DTO_SUFFIX = "Entity";
	protected static readonly string REFERENCE_SUFFIX = "Id";
	protected static readonly string DEFAULT_NH_COLLECTION_TYPE = "IList";
	protected static readonly string DEFAULT_NH_COLLECTION_MAPPING_TYPE = "bag";
	//protected static readonly string DEFAULT_NH_COLLECTION_TYPE = "ISet";
	//protected static readonly string DEFAULT_NH_COLLECTION_MAPPING_TYPE = "set";
	protected static readonly string DEFAULT_DTO_COLLECTION_TYPE = "IList";
	protected static readonly string DEFAULT_ID_ALIAS = "Id";
	protected static readonly string DEFAULT_VERSION_ALIAS = "ModificationCounter";
	protected static readonly string DEFAULT_NAME_ALIAS = "Name";
	protected static readonly string FK_PREFIX = "LNKID_";
	protected static readonly string FK_ID_SUFFIX = "Id";
	protected static readonly string NAME_COLUMN_NAME = "Name";
	protected static readonly string CODE_COLUMN_NAME = "Code";
	protected static readonly string CREATED_COLUMN_NAME = "Created";
	protected static readonly string DELETED_COLUMN_NAME = "Deleted";
	protected static readonly string ARCHIVED_COLUMN_NAME = "Archived";
	protected static readonly string REF_ID_COLUMN_NAME = "RefId";
	protected static readonly string WORKFLOW_STATUS_COLUMN_NAME = "WorkflowStatusId";
	protected static readonly string PENDING_ENTITY_COLUMN_NAME = "PendingEntityId";
	protected static readonly string WORKFLOW_STATUS_ENUM_TYPE = "WorkflowStatus";
	protected static readonly string WORKFLOW_STATUS_ALIAS = "WorkflowStatus";
	protected static readonly string SPACE_STR = " ";
	protected static readonly string NULL_SUFFIX = "?";
	#endregion

	#region constructor
	/// <summary>
	/// Initializes user data maps
	/// </summary>
	/// <param name="context"></param>
	public BaseTemplate(ZeusContext context) : base(context)
	{
		if (_nhSupportedTypeMap == null)
		{
			_nhSupportedTypeMap = DnpUtils.ReadUserMap(USER_MAP_FILENAME, USER_MAP_CSHARP_TO_NHIBERNATE_SUPPORTED_TYPE, MyMeta);
			_nhNullableTypeMap = DnpUtils.ReadUserMap(USER_MAP_FILENAME, USER_MAP_CSHARP_TO_NULLABLE_TYPE, MyMeta);
			_nhMappingTypeMap = DnpUtils.ReadUserMap(USER_MAP_FILENAME, USER_MAP_CSHARP_TO_NHIBERNATE_MAPPING_TYPE, MyMeta);
		}
	}
	#endregion

	#region helper methods

	/// <summary>
	/// Gets the input from the common input UI
	/// </summary>
	protected void GetInput()
	{
		if (_dbName == null)
		{
			_dbName = input["database"].ToString();
			_selectedTables = input["tables"] as ArrayList;
			_selectedViews = input["views"] as ArrayList;
			_baseOutputPath = input["baseOutputPath"].ToString();
			_baseNamespace = input["baseNamespace"].ToString();
			_baseAssemblyName = input["baseAssemblyName"].ToString();
			_schemaName = input["schemaName"].ToString();
			_prefix = input["memberPrefix"].ToString();
			_versionColumnName = input["versionColumnName"].ToString();
			_genNhMappings = (bool)input["chkGenNhMappings"];
			_genDtos = (bool)input["chkGenDtos"];
			_genEnums = (bool)input["chkGenEnums"];
			_genSpringDaoConfig = (bool)input["chkGenSpringDaoConfig"];
			_genDaoUnitTests = (bool)input["chkGenDaoUnitTests"];
			//_genDaos = (bool)input["chkGenDaos"];
			//_genAdapter = (bool)input["chkGenAdapter"];
			//_createReadOnly = (bool)input["chkReadOnly"];
			//_generateEqualsHashCode = (bool)input["chkGenEqualsHashCode"];
			//_constructorUseNull = (bool)input["chkConstructorUseNull"];
			_constructorUseNull = true;
			//_classRefForFk = (bool)input["chkClassRefForFk"];
			//_enableLazyClass = (bool)input["chkLazyClass"];
			_enableLazyClass = true;
			_separateAssembliesForGroupings = false;
			_logicalGroupName = String.Empty;
		}
	}

	/// <summary>
	/// A method to wrap simple logging. The implementation outputs
	/// to the Zeus context.
	/// </summary>
	/// <param name="output"></param>
	protected void Log(object output)
	{
		if (output is Exception)
		{
			context.Log.Write(output as Exception);
		}
		else if (output != null)
		{
			context.Log.Write(output.ToString());
		}
		else
		{
			context.Log.Write("null");
		}
	}

	/// <summary>
	/// Checks a custom property to determine if a particular table
	/// should be ignored.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected virtual bool IgnoreTable(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_IGNORE];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Checks a custom property to determine if a particular column
	/// should be ignored.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected virtual bool IgnoreColumn(IColumn col)
	{
		bool retVal = false;
		if (col != null)
		{
			IProperty prop = col.Properties[CP_IGNORE];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns the parent table, if it exists, for the given table. The logic looks
	/// at the primary key to see if it is also a foreign key. The implementation currently
	/// only suppport single column primary keys.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected ITable GetBaseTable(ITable table)
	{
		ITable retVal = null;
		if (table != null && table.PrimaryKeys.Count == 1 && table.ForeignKeys.Count > 0)
		{
			IColumn col = table.PrimaryKeys[0];
			if (col.IsInForeignKey)
			{
				foreach (IForeignKey fk in table.ForeignKeys)
				{
					if (fk.PrimaryTable != table && fk.ForeignColumns.Count == 1 && fk.ForeignColumns[0].Name == col.Name)
					{
						retVal = fk.PrimaryTable;
					}
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether the attribute for a column associated with a foreign key
	/// should be generated as a class reference to the type associated with the
	/// referencing table or a value attribute for the foreign key. This depends on
	/// several factors. The column must be a foreign key and not part of the primary key.
	/// The ClassRefForFk property must either be specified as the default behavior or
	/// explicityly set as a metadata custom property.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsReferenceColumn(IColumn col)
	{
		bool retVal = false;
		if (col.IsInForeignKey && !col.IsInPrimaryKey)
		{
			IColumn parentCol = GetParentColumn(col.Table);
			if (parentCol != null && parentCol.Name == col.Name)
			{
				retVal = true;
			}
			else if (IsWorkflowTable(col.Table) && col.Name == PENDING_ENTITY_COLUMN_NAME)
			{
				retVal = true;
			}
			else
			{
				IProperty prop = col.Properties[CP_CLASS_REF_FOR_FK];
				if (prop != null && prop.Value != null && prop.Value.Length != 0)
				{
					retVal = Convert.ToBoolean(prop.Value);

				}
				else if (_classRefForFk)
				{
					retVal = true;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether the column references a table that represents
	/// and enumeration.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsEnumeratedColumn(IColumn col)
	{
		bool retVal = false;
		IProperty prop = col.Properties[CP_ENUM];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			retVal = Convert.ToBoolean(prop.Value);
		}
		else
		{
			retVal = GetEnumerationType(col) != null;
		}
		return retVal;
	}

	/// <summary>
	/// Checks the custom property on the column to determine whether
	/// to generate a finder method for the given column.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetFinderColumnValue(IColumn col)
	{
		string retVal = null;
		IProperty prop = col.Properties[CP_FINDER_COLUMN];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			retVal = prop.Value;
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether the table represents an enumeration. The implementation
	/// checks the table name and the table alias.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool IsEnumerationTable(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			if (GetSiteBaseData().ContainsKey(table.Name))
			{
				retVal = GetSiteBaseData()[table.Name].IsEnum;
			}
			else
			{
				IProperty prop = table.Properties[CP_ENUM];
				if (prop != null && prop.Value != null && prop.Value.Length != 0)
				{
					retVal = Convert.ToBoolean(prop.Value);
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Gets the name of the enumeration type for the given column. Checks to see if
	/// the column is a foreign key to a table which represents an enumeration. If the
	/// column does not represent an enumeration type, the method will return null.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetEnumerationType(IColumn col)
	{
		string retVal = null;

		IProperty prop = col.Properties[CP_ENUM];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			if (!Convert.ToBoolean(prop.Value))
			{
				return null;
			}
		}

		if (col.IsInForeignKey)
		{
			foreach (IForeignKey fk in col.Table.ForeignKeys)
			{
				if (fk.ForeignColumns.Count == 1
					&& fk.ForeignTable == col.Table 
					&& fk.ForeignColumns[0].Name == col.Name
					&& IsEnumerationTable(fk.PrimaryTable))
				{
					retVal = TableToEntity(fk.PrimaryTable.Alias);
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Gets the logical grouping (namespace and assembly suffix) for the enumeration
	/// type associated with the given table.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetLogicalGroup(ITable table)
	{
		string retVal = null;
		if (table != null)
		{
			if (GetSiteBaseData().ContainsKey(table.Name))
			{
				retVal = GetSiteBaseData()[table.Name].LogicalGroup;
			}
			else
			{
				IProperty prop = table.Properties[CP_LOGICAL_GROUP];
				if (prop != null && prop.Value != null && prop.Value.Length != 0)
				{
					retVal = prop.Value;
				}
			}
		}
		return retVal;
	}


	/// <summary>
	/// Gets the logical grouping (namespace and assembly suffix) for the enumeration
	/// type associated with the given column.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetLogicalGroup(IColumn col)
	{
		string retVal = null;
		if (col.IsInForeignKey)
		{
			foreach (IForeignKey fk in col.Table.ForeignKeys)
			{
				if (fk.ForeignColumns.Count == 1
					&& fk.ForeignTable == col.Table
					&& fk.ForeignColumns[0].Name == col.Name)
					//&& IsEnumerationTable(fk.PrimaryTable))
				{
					retVal = GetLogicalGroup(fk.PrimaryTable);
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Checks the table for the presence of Created and Deleted columns
	/// to determine whether the table supports the IVersionedEntity interface.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool IsVersionedTable(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			bool createdFound = false, deletedFound = false;
			foreach (IColumn col in table.Columns)
			{
				if (col.Name == CREATED_COLUMN_NAME)
				{
					createdFound = true;
				}
				else if (col.Name == DELETED_COLUMN_NAME)
				{
					deletedFound = true;
				}
				if (createdFound && deletedFound)
				{
					retVal = true;
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Checks the table for the presence of RefId, Created and Archived columns
	/// to determine whether the table supports the IArchivedEntity interface.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool IsArchivedTable(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			bool refIdFound = false, createdFound = false, archivedFound = false;
			foreach (IColumn col in table.Columns)
			{
				if (col.Name == REF_ID_COLUMN_NAME)
				{
					refIdFound = true;
				}
				else if (col.Name == CREATED_COLUMN_NAME)
				{
					createdFound = true;
				}
				else if (col.Name == ARCHIVED_COLUMN_NAME)
				{
					archivedFound = true;
				}
				if (refIdFound && createdFound && archivedFound)
				{
					retVal = true;
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Checks the table for the presence of WorkflowStatusId and
	/// PendingEntityId columns to determine whether the table supports
	/// the IWorkflowEntity interface.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool IsWorkflowTable(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			bool workflowFound = false, pendingFound = false;
			foreach (IColumn col in table.Columns)
			{
				if (col.Name == WORKFLOW_STATUS_COLUMN_NAME)
				{
					workflowFound = true;
				}
				else if (col.Name == PENDING_ENTITY_COLUMN_NAME)
				{
					pendingFound = true;
				}
				if (workflowFound && pendingFound)
				{
					retVal = true;
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether a finder method that returns a collection should be generated
	/// for the given column
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsCollectionFinderColumn(IColumn col)
	{
		bool retVal = false;
		string propVal = GetFinderColumnValue(col);
		if (propVal != null && propVal.ToLower() == FINDER_COLUMN_VAL_COLLECTION.ToLower())
		{
			retVal = true;
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether a finder method that returns a unique result should be generated
	/// for the given column
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsUniqueResultFinderColumn(IColumn col)
	{
		bool retVal = false;
		string propVal = GetFinderColumnValue(col);
		if (propVal != null && propVal.ToLower() == FINDER_COLUMN_VAL_UNIQUE_RESULT.ToLower())
		{
			retVal = true;
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether database operations should cascade to the reference class for this column
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsCascadeColumn(IColumn col)
	{
		bool retVal = IsReferenceColumn(col);
		if (retVal)
		{
			IProperty prop = col.Properties[CP_CASCADE];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
			else
			{
				retVal = false;
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether class references should be loaded by default
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsLazyColumn(IColumn col)
	{
		bool retVal = IsReferenceColumn(col);
		if (retVal)
		{
			IProperty prop = col.Properties[CP_LAZY];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
			else
			{
				retVal = true;
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns the table reference set in the MyMeta custom property for
	/// the given column. This custom property can be used to simulate a foreign
	/// key reference.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetTableRefForColumn(IColumn col)
	{
		string retVal = null;
		IProperty prop = col.Properties[CP_TABLE_REF];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			retVal = prop.Value;
		}
		return retVal;
	}

	/// <summary>
	/// Gets the special Code column for the table. This value can be specified
	/// as a custom table property or will matched to a column named
	/// CODE_COLUMN_NAME. If the default behavior is not desired, a
	/// value of None can be specified in the custom table property.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected string GetCodeColumn(ITable table)
	{
		string retVal = null;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_CODE_COLUMN];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = prop.Value;
			}
		}
		if (retVal != null && retVal.ToLower() == CODE_COLUMN_VAL_NONE.ToLower())
		{
			retVal = null;
		}
		//else if (retVal == null)
		//{
		//	foreach (Column col in table.Columns)
		//	{
		//		if (col.Name == CODE_COLUMN_NAME)
		//		{
		//			retVal = col.Name;
		//			break;
		//		}
		//	}
		//}
		return retVal;
	}

	/// <summary>
	/// Gets the special Name column for the table. This value can be specified
	/// as a custom table property or will matched to a column named
	/// NAME_COLUMN_NAME. If the default behavior is not desired, a
	/// value of None can be specified in the custom table property.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected string GetNameColumn(ITable table)
	{
		string retVal = null;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_NAME_COLUMN];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = prop.Value;
			}
		}
		//if (retVal != null && retVal.ToLower() == NAME_COLUMN_VAL_NONE.ToLower())
		//{
		//	retVal = null;
		//}
		//else if (retVal == null)
		//{
		//	foreach (Column col in table.Columns)
		//	{
		//		if (col.Name == NAME_COLUMN_NAME)
		//		{
		//			retVal = col.Name;
		//			break;
		//		}
		//	}
		//}
		return retVal;
	}

	/// <summary>
	/// Gets the special Enum column for the table. This value can be specified
	/// as a custom table property.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected string GetEnumColumn(ITable table)
	{
		string retVal = null;
		if (table != null && IsEnumerationTable(table))
		{
			IProperty prop = table.Properties[CP_ENUM_COLUMN];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = prop.Value;
			}
			if (retVal == null)
			{
				retVal = GetNameColumn(table);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Gets the prefix to prepend to the generated entity class for the given table
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected string GetEntityPrefix(ITable table)
	{
		string retVal = null;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_ENTITY_PREFIX];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = prop.Value;
			}
		}
		return retVal;
	}

	/// <summary>
	/// Returns whether the FetchAll method should be exposed for the table type
	/// at the adapter level. The default is not to expose the FetchAll method since
	/// a these calls can be resource intensive for large tables.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool GetFetchAll(ITable table)
	{
		bool retVal = false;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_FETCH_ALL];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Checks the CP_PARENT_COLUMN table custom property to determine
	/// the foreign key column for the parent entity. This method is
	/// used in conjunction with dependency tables which typically
	/// represent child collections for a parent entity.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected IColumn GetParentColumn(ITable table)
	{
		IColumn retVal = null;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_PARENT_COLUMN];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				foreach (IColumn col in table.Columns)
				{
					if (col.Name == prop.Value && col.IsInForeignKey && col.ForeignKeys.Count == 1)
					{
						retVal = col;
						break;
					}
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// This is a convenience method that returns the parent table. This
	/// method is used in conjunction with dependency tables which typically
	/// represent child collections for a parent entity. See the
	/// GetParentColumn method.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected ITable GetParentTable(ITable table)
	{
		ITable retVal = null;
		IColumn col = GetParentColumn(table);
		if (col != null)
		{
			retVal = col.ForeignKeys[0].PrimaryTable;
		}
		return retVal;
	}

	/// <summary>
	/// Checks the CP_LAZY table property to determine whether the generated
	/// object should use lazy initialization by default.
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected bool IsLazy(ITable table)
	{
		bool retVal = _enableLazyClass;
		if (table != null)
		{
			IProperty prop = table.Properties[CP_LAZY];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Get the namespace for the table to which the given foreign key column corresponds
	/// </summary>
	/// <param name="col"></param>
	/// <param name="pathPrefix"></param>
	/// <returns></returns>
	protected string GetNamespace(IColumn col, string pathPrefix)
	{
		string retVal = null;
		if (col.IsInForeignKey)
		{
			foreach (IForeignKey fk in col.Table.ForeignKeys)
			{
				if (fk.ForeignColumns.Count == 1
					&& fk.ForeignTable == col.Table
					&& fk.ForeignColumns[0].Name == col.Name)
				{
					retVal = GetNamespace(fk.PrimaryTable, pathPrefix);
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Get the assembly name for the table to which the given foreign key column corresponds
	/// </summary>
	/// <param name="col"></param>
	/// <param name="pathPrefix"></param>
	/// <returns></returns>
	protected string GetAssemblyName(IColumn col, string pathPrefix)
	{
		string retVal = null;
		if (col.IsInForeignKey)
		{
			foreach (IForeignKey fk in col.Table.ForeignKeys)
			{
				if (fk.ForeignColumns.Count == 1
					&& fk.ForeignTable == col.Table
					&& fk.ForeignColumns[0].Name == col.Name)
				{
					retVal = GetAssemblyName(fk.PrimaryTable, pathPrefix);
					break;
				}
			}
		}
		return retVal;
	}

	/// <summary>
	/// Get the namespace for the given table
	/// </summary>
	/// <param name="table"></param>
	/// <param name="pathPrefix"></param>
	/// <returns></returns>
	protected string GetNamespace(ITable table, string pathPrefix)
	{
		string baseNameSpace = BaseNamespace;
		if (GetSiteBaseData().ContainsKey(table.Name))
		{
			baseNameSpace = GetSiteBaseData()[table.Name].BaseNamespace;
		}
		string retVal = baseNameSpace + (baseNameSpace.EndsWith(".") ? String.Empty : ".")
			+ DnpUtils.SetPascalCase(pathPrefix).Replace(Path.DirectorySeparatorChar, '.');
		string logicalGrouping = GetLogicalGroup(table);
		if (!String.IsNullOrEmpty(logicalGrouping))
		{
			retVal += "." + DnpUtils.SetPascalCase(logicalGrouping);
		}
		return retVal;
	}

	/// <summary>
	/// Get the assembly name for the given table
	/// </summary>
	/// <param name="table"></param>
	/// <param name="pathPrefix"></param>
	/// <returns></returns>
	protected string GetAssemblyName(ITable table, string pathPrefix)
	{
		string baseAssemblyName = BaseAssemblyName;
		if (GetSiteBaseData().ContainsKey(table.Name))
		{
			baseAssemblyName = GetSiteBaseData()[table.Name].BaseAssemblyName;
		}
		string retVal = baseAssemblyName + (baseAssemblyName.EndsWith(".") ? String.Empty : ".")
			+ DnpUtils.SetPascalCase(pathPrefix).Replace(Path.DirectorySeparatorChar, '.');
		if (_separateAssembliesForGroupings)
		{
			string logicalGrouping = GetLogicalGroup(table);
			if (!String.IsNullOrEmpty(logicalGrouping))
			{
				retVal += "." + DnpUtils.SetPascalCase(logicalGrouping);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Gets a collection type custom property. The property name is formed by
	/// concatenating the CP_COLLECTION_PREFIX, the collection index and the
	/// desired property.
	/// </summary>
	/// <param name="table"></param>
	/// <param name="index"></param>
	/// <param name="propSuffix"></param>
	/// <returns></returns>
	protected string GetCollectionProperty(ITable table, int index, string propSuffix)
	{
		string retVal = null;
		IProperty prop = table.Properties[CP_COLLECTION_PREFIX + index + propSuffix];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			retVal = prop.Value;
		}
		return retVal;
	}

	/// <summary>
	/// Converts the name of the given column the appropriate form for
	/// a private member variable.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string ColumnToMemberVariable(IColumn col)
	{
		return _prefix + DnpUtils.SetCamelCase(GetColumnAlias(col));
	}
	
	/// <summary>
	/// Converts the name of the given column the appropriate form for
	/// a property accessor.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string ColumnToPropertyName(IColumn col)
	{
		return DnpUtils.SetPascalCase(GetColumnAlias(col));
	}

	/// <summary>
	/// Converts the name of the given column the appropriate form for
	/// a method parameter.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string ColumnToArgumentName(IColumn col)
	{
		return DnpUtils.SetCamelCase(GetColumnAlias(col));
	}

	/// <summary>
	/// Get alias for column name by applying various database schema conventions.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string GetColumnAlias(IColumn col)
	{
		string cName = col.Alias.Replace(SPACE_STR, String.Empty);
		if (col.IsInPrimaryKey)
		{
			cName = DEFAULT_ID_ALIAS;
		}
		else if (col.Name == VersionColumnName)
		{
			cName = DEFAULT_VERSION_ALIAS;
		}
		else if (IsWorkflowTable(col.Table) && col.Name == WORKFLOW_STATUS_COLUMN_NAME)
		{
			cName = WORKFLOW_STATUS_ALIAS;
		}
		//else if (col.Alias.StartsWith(FK_PREFIX))
		//{
		//	// strip the LNKID_ off column names
		//	cName = col.Alias.Substring(FK_PREFIX.Length);
		//	if (!IsReferenceColumn(col))
		//	{
		//		// add _ID to to column names that are not mapped to a class reference
		//		cName += FK_ID_SUFFIX;
		//	}
		//}
		else if (col.Alias != FK_ID_SUFFIX && col.Alias.EndsWith(FK_ID_SUFFIX))
		{
			if (IsReferenceColumn(col) || IsEnumeratedColumn(col))
			{
				// remove FK_ID_SUFFIX from column names that are not mapped to a class reference
				cName = cName.Substring(0, cName.Length - FK_ID_SUFFIX.Length);
			}
		}
		else if (col.Name == GetNameColumn(col.Table))
		{
			cName = DEFAULT_NAME_ALIAS;
		}
		//else if (col.Table != null && col.Table.Alias.Replace(SPACE_STR, String.Empty) == cName)
		//{
		//	cName += NAME_SUFFIX;
		//}
		//else if (col.View != null && col.View.Alias.Replace(SPACE_STR, String.Empty) == cName)
		//{
		//	cName += NAME_SUFFIX;
		//}
		return cName;
	}

	/// <summary>
	/// Returns the number of required columns for the given list of columns
	/// </summary>
	/// <param name="cols"></param>
	/// <returns></returns>
	protected int CountRequiredFields(IColumns cols)
	{
		return cols.Count - CountNullableFields(cols);
	}

	/// <summary>
	/// Returns the number of nullable columns for the given list of columns
	/// </summary>
	/// <param name="cols"></param>
	/// <returns></returns>
	protected int CountNullableFields(IColumns cols)
	{
		int i = 0;
		foreach (IColumn col in cols)
		{
			if (IgnoreColumn(col))
			{
				continue;
			}
			if (col.IsNullable)
			{
				i++;
			}
		}
		return i;
	}
	
	/// <summary>
	/// Returns the number of unique columns for the given list of columns
	/// </summary>
	/// <param name="cols"></param>
	/// <returns></returns>
	protected int CountUniqueFields(IColumns cols)
	{
		int i = 0;
		foreach (IColumn col in cols)
		{
			if (IgnoreColumn(col))
			{
				continue;
			}
			if (!col.IsNullable && col.IsInPrimaryKey)
			{
				i++;
			}
		}
		return i;
	}

	/// <summary>
	/// Returns whether the given column is nullable. The implementation checks
	/// the nullable attribute of the column type but overrides the value by
	/// checking the CP_NULLABLE custom column property.
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected bool IsColumnNullable(IColumn col)
	{
		bool retVal = col.IsNullable;
		if (retVal)
		{
			IProperty prop = col.Properties[CP_NULLABLE];
			if (prop != null && prop.Value != null && prop.Value.Length != 0)
			{
				retVal = Convert.ToBoolean(prop.Value);
			}
		}
		return retVal;
	}

	/// <summary>
	/// Transform the C# mapping type to the supported NHibernate type
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string ColumnToNHibernateSupportedType(IColumn col)
	{
		return ColumnToNHibernateSupportedType(col, true);
	}

	/// <summary>
	/// Transform the C# mapping type to the supported NHibernate type
	/// </summary>
	/// <param name="col"></param>
	/// <param name="allowNullables"></param>
	/// <returns></returns>
	protected string ColumnToNHibernateSupportedType(IColumn col, bool allowNullables)
	{
		string retVal = col.LanguageType;

		string enumType = GetEnumerationType(col);
		IProperty prop = col.Properties[CP_DATA_TYPE];
		if (prop != null && prop.Value != null && prop.Value.Length != 0)
		{
			retVal = prop.Value;
		}
		else if (enumType != null)
		{
			retVal = enumType;
			if (IsColumnNullable(col))
			{
				retVal += NULL_SUFFIX;
			}
		}
		if (IsColumnNullable(col) && _nhNullableTypeMap.ContainsKey(retVal))
		{
			retVal = retVal += NULL_SUFFIX;
		}
		else if (_nhSupportedTypeMap.ContainsKey(retVal))
		{
			retVal = _nhSupportedTypeMap[retVal] as string;
		}

		return retVal;
	}

	/// <summary>
	/// Transform the column type to the appropriate type for the NHibernate mapping
	/// </summary>
	/// <param name="col"></param>
	/// <returns></returns>
	protected string ColumnToNHibernateMappingType(IColumn col)
	{
		string retVal = ColumnToNHibernateSupportedType(col, false);
		if (IsEnumeratedColumn(col))
		{
			retVal = GetEnumerationType(col);
		}
		if (retVal.EndsWith(NULL_SUFFIX))
		{
			retVal = retVal.Substring(0, retVal.Length - NULL_SUFFIX.Length);
		}
		if (_nhMappingTypeMap.ContainsKey(retVal))
		{
			retVal = _nhMappingTypeMap[retVal] as string;
		}
		return retVal;
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// the associated data transfer object.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToDto(string alias)
	{
		string retVal = null;
		//if (GetEnumerationTables().Contains(alias))
		//{
		//	retVal = TableToEntity(alias);
		//}
		//else
		//{
			retVal = TableToEntity(alias) + DTO_SUFFIX;
		//}
		return retVal;
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// the associated DAO interface.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToDaoInterface(string alias)
	{
		return INTERFACE_PREFIX + TableToEntity(alias) + DAO_SUFFIX;
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// the associated entity.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToEntity(string alias)
	{
		return DnpUtils.SetPascalCase(alias.Replace(SPACE_STR, String.Empty));
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// the associated DAO.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToDao(string alias)
	{
		return TableToEntity(alias) + DAO_SUFFIX;
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// the associated NHibernate model type.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToNhModel(string alias)
	{
		return TableToEntity(alias);
	}

	/// <summary>
	/// Converts the name of the given table alias to the appropriate form for
	/// an associated class reference private member variable.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string TableToMemberVariable(string alias)
	{
		return _prefix + DnpUtils.SetCamelCase(alias);
	}

	/// <summary>
	/// Returns the default collection type the given table
	/// </summary>
	/// <param name="table"></param>
	/// <returns></returns>
	protected string TableToDtoCollection(ITable table)
	{
		return TableToDtoCollection(table, DEFAULT_DTO_COLLECTION_TYPE);
	}

	/// <summary>
	/// Returns the default collection type the given table
	/// </summary>
	/// <param name="table"></param>
	/// <param name="collectionType"></param>
	/// <returns></returns>
	protected string TableToDtoCollection(ITable table, string collectionType)
	{
		return TableToDtoCollection(table, collectionType, false);
	}

	/// <summary>
	/// Returns the default collection type the given table
	/// </summary>
	/// <param name="table"></param>
	/// <param name="collectionType"></param>
	/// <param name="enumeration"></param>
	/// <returns></returns>
	protected string TableToDtoCollection(ITable table, string collectionType, bool enumeration)
	{
		return String.Format("{0}<{1}>", collectionType, enumeration ? TableToEntity(table.Alias) : TableToDto(table.Alias));
	}

	/// <summary>
	/// Builds the name of the adapter based on the logical group name
	/// </summary>
	/// <returns></returns>
	protected string LogicalGroupToAdapter()
	{
		return "Generated" + DnpUtils.SetPascalCase(_logicalGroupName) + "Adapter";
	}

	/// <summary>
	/// Builds the name of the adapter interface based on the logical group name
	/// </summary>
	/// <returns></returns>
	protected string LogicalGroupToAdapterInterface()
	{
		return INTERFACE_PREFIX + LogicalGroupToAdapter();
	}

	/// <summary>
	/// Gets the name of the Id property
	/// </summary>
	/// <returns></returns>
	protected string GetIdPropertyName()
	{
		return DnpUtils.SetPascalCase(DEFAULT_ID_ALIAS);
	}

	/// <summary>
	/// Gets the name of version property
	/// </summary>
	/// <returns></returns>
	protected string GetVersionPropertyName()
	{
		return DnpUtils.SetPascalCase(DEFAULT_VERSION_ALIAS);
	}

	/// <summary>
	/// Converts the given table alias to a pluralized form. This method only
	/// handles the most common cases.
	/// </summary>
	/// <param name="alias"></param>
	/// <returns></returns>
	protected string PluralizeTableName(string alias)
	{
		string retVal = alias.Replace(SPACE_STR, String.Empty);
		if (alias.EndsWith("y"))
		{
			retVal = retVal.Substring(0, retVal.Length - 1) + "ies";
		}
		else if (!alias.EndsWith("List"))
		{
			retVal += "s";
		}
		return retVal;
	}

	/// <summary>
	/// This method will return a dictionary of database tables that are to be represented
	/// as enumerations. The key for the dictionary is the enumeration table name, and the
	/// dictionary value is the logical grouping for the entry. The implementation reads the
	/// list in from an XML file called SITE_BASE_DATA_FILE_NAME from the same directory as
	/// the MyGeneration settings files.
	/// </summary>
	/// <returns></returns>
	protected IDictionary<string, SiteBaseTable> GetSiteBaseData()
	{
		if (_siteBaseData == null)
		{
			try
			{
				_siteBaseData = new Dictionary<string, SiteBaseTable>();
				string path = MyMeta.DbTargetMappingFileName;
				path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
				XPathDocument xpDoc = new XPathDocument(Path.Combine(path, SITE_BASE_DATA_FILE_NAME));
				XPathNavigator xpNav = xpDoc.CreateNavigator();
				XPathNodeIterator nodes = xpNav.Select("/SiteBaseData/Table");
				while (nodes.MoveNext())
				{
					SiteBaseTable table = new SiteBaseTable();
					table.Name = nodes.Current.Value;
					table.Alias = nodes.Current.GetAttribute("Alias", xpNav.NamespaceURI);
					table.BaseNamespace = nodes.Current.GetAttribute("BaseNamespace", xpNav.NamespaceURI);
					table.BaseAssemblyName = nodes.Current.GetAttribute("BaseAssemblyName", xpNav.NamespaceURI);
					table.IsEnum = Convert.ToBoolean(nodes.Current.GetAttribute("IsEnum", xpNav.NamespaceURI));
					table.LogicalGroup = nodes.Current.GetAttribute("LogicalGroup", xpNav.NamespaceURI);
					_siteBaseData[table.Name] = table;
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}
		return _siteBaseData;
	}

	#endregion

	#region language specific header output methods

	protected void OutputCSharpHeader()
	{
		OutputHeader("// ", " //");
	}

	protected void OutputXmlHeader()
	{
		OutputHeader("<!-- ", " -->", "--", "==");
	}

	protected void OutputHeader(string linePrefix, string lineSuffix)
	{
		OutputHeader(linePrefix, lineSuffix, null, null);
	}

	protected void OutputHeader(string linePrefix, string lineSuffix, string search, string replace)
	{
		if (_headerText == null)
		{
			ReadHeaderFile();
		}
		using (TextReader reader = new StringReader(_headerText))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				output.write(linePrefix);
				if (search != null && replace != null)
				{
					output.write(line.Replace(search, replace));
				}
				else
				{
					output.write(line);
				}
				output.writeln(lineSuffix);
			}
		}
	}

	private void ReadHeaderFile()
	{
		if (_headerText == null)
		{
			StringBuilder sb = new StringBuilder();
			string path = MyMeta.DbTargetMappingFileName;
			path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
			using (TextReader reader = new StreamReader(Path.Combine(path, HEADER_TXT_FILE)))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					sb.Append(line);
					sb.Append("\r\n");
				}
			}
			_headerText = sb.ToString();
		}
	}

	#endregion

	#region private attributes

	private static ArrayList _selectedTables;
	private static ArrayList _selectedViews;
	private static string _dbName;
	private static string _schemaName;
	private static string _baseNamespace;
	private static string _baseAssemblyName;
	private static string _logicalGroupName;
	private static string _baseOutputPath;
	private static string _prefix;
	private static string _versionColumnName;
	private static bool _genDtos;
	private static bool _genNhMappings;
	private static bool _genEnums;
	private static bool _genSpringDaoConfig;
	private static bool _genDaoUnitTests;
	private static bool _genDaos;
	private static bool _genAdapter;
	private static bool _createReadOnly;
	private static bool _generateEqualsHashCode;
	private static bool _constructorUseNull;
	private static bool _classRefForFk;
	private static bool _enableLazyClass;
	private static bool _separateAssembliesForGroupings;
	private static Hashtable _nhSupportedTypeMap;
	private static Hashtable _nhNullableTypeMap;
	private static Hashtable _nhMappingTypeMap;
	private static IDictionary<string,SiteBaseTable> _siteBaseData;
	private static string _headerText;

	#endregion

	#region protected properties

	protected ArrayList SelectedTables
	{
		get { return _selectedTables; }
		set { _selectedTables = value; }
	}

	protected ArrayList SelectedViews
	{
		get { return _selectedViews; }
		set { _selectedViews = value; }
	}

	protected string DbName
	{
		get { return _dbName; }
		set { _dbName = value; }
	}

	protected string SchemaName
	{
		get { return _schemaName; }
		set { _schemaName = value; }
	}

	protected string BaseOutputPath
	{
		get { return _baseOutputPath; }
		set { _baseOutputPath = value; }
	}

	protected string BaseNamespace
	{
		get { return _baseNamespace; }
		set { _baseNamespace = value; }
	}

	protected string BaseAssemblyName
	{
		get { return _baseAssemblyName; }
		set { _baseAssemblyName = value; }
	}

	protected string Prefix
	{
		get { return _prefix; }
		set { _prefix = value; }
	}

	protected string VersionColumnName
	{
		get { return _versionColumnName; }
		set { _versionColumnName = value; }
	}

	public string LogicalGroupName
	{
		get { return _logicalGroupName; }
		set { _logicalGroupName = value; }
	}

	protected bool GenSpringDaoConfig
	{
		get { return _genSpringDaoConfig; }
	}

	protected bool GenNhMappings
	{
		get { return _genNhMappings; }
	}

	public bool GenDtos
	{
		get { return _genDtos; }
	}

	public bool GenDaoUnitTests
	{
		get { return _genDaoUnitTests; }
	}

	public bool GenDaos
	{
		get { return _genDaos; }
	}

	public bool GenAdapter
	{
		get { return _genAdapter; }
	}

	public bool GenEnums
	{
		get { return _genEnums; }
	}

	protected bool CreateReadOnly
	{
		get { return _createReadOnly; }
		set { _createReadOnly = value; }
	}

	protected bool GenerateEqualsHashCode
	{
		get { return _generateEqualsHashCode; }
		set { _generateEqualsHashCode = value; }
	}

	protected bool ConstructorUseNull
	{
		get { return _constructorUseNull; }
		set { _constructorUseNull = value; }
	}

	protected bool ClassRefForFk
	{
		get { return _classRefForFk; }
		set { _classRefForFk = value; }
	}

	protected bool EnableLazyClass
	{
		get { return _enableLazyClass; }
		set { _enableLazyClass = value; }
	}

	#endregion
}

public class SiteBaseTable
{
	public string Name;
	public string Alias;
	public string BaseNamespace;
	public string BaseAssemblyName;
	public string LogicalGroup;
	public bool IsEnum;
}