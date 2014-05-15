// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;
using DigitalBeacon.Model;

namespace DigitalBeacon.Business
{
	public class EntityDependencyException : ServiceException
	{
		private long _entityId;
		private Type _entityType;

		public EntityDependencyException(IBaseEntity entity)
			: base("{0} with Id [{1}] cannot be deleted because one or more other objects depend on it.", entity.GetType().Name, entity.Id)
		{
			_entityId = entity.Id;
			_entityType = entity.GetType();
		}

		public Type EntityType
		{
			get { return _entityType; }
			set { _entityType = value; }
		}

		public long EntityId
		{
			get { return _entityId; }
			set { _entityId = value; }
		}
	}
}
