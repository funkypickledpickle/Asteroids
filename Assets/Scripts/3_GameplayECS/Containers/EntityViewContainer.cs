using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.GameplayECS.Components;
using Asteroids.Tools;
using Asteroids.ValueTypeECS.Entities;
using Asteroids.ValueTypeECS.EntityContainer;
using Asteroids.ValueTypeECS.EntityGroup;
using UnityEngine;

namespace Asteroids.GameplayECS.Containers
{
    public interface IEntityViewContainer
    {
        public int? GetEntityId(GameObject view);
    }

    public class EntityViewContainer : IEntityViewContainer
    {
        private Dictionary<GameObject, int> _entityDictionary;
        private Dictionary<int, GameObject> _reverseDictionary;

        public EntityViewContainer(World world, IInstanceSpawner instanceSpawner)
        {
            var viewGroup = instanceSpawner.Instantiate<EntityGroupBuilder>().RequireComponent<ViewComponent>().Build();
            viewGroup.SubscribeToEntityAddedEvent(ViewAddedHandler);

            var viewRemovedGroup = instanceSpawner.Instantiate<EntityGroupBuilder>()
               .RequireComponent<ViewComponent>()
               .RequireComponent<DestroyedComponent>().Build();
            viewRemovedGroup.SubscribeToEntityAddedEvent(ViewRemovedHandler);

            _entityDictionary = viewGroup.IdsList.Select(id => (world.GetEntity(id).GetComponent<ViewComponent>().View, id)).ToDictionary(key => key.View, value => value.id);
            _reverseDictionary = _entityDictionary.ToDictionary(key => key.Value, value => value.Key);
        }

        private void ViewAddedHandler(ref Entity referenced)
        {
            var gameObject = referenced.GetComponent<ViewComponent>().View;
            var entityId = referenced.Id;
            _entityDictionary.Add(gameObject, entityId);
            _reverseDictionary.Add(entityId, gameObject);
        }

        private void ViewRemovedHandler(ref Entity referenced)
        {
            var gameObject = referenced.GetComponent<ViewComponent>().View;
            var entityId = referenced.Id;
            _entityDictionary.Remove(gameObject);
            _reverseDictionary.Remove(entityId);
        }

        public int? GetEntityId(GameObject view)
        {
            if (_entityDictionary.TryGetValue(view, out var entityId))
            {
                return entityId;
            }

            return null;
        }
    }
}
