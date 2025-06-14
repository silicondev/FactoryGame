﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FactoryGame.Source.Systems.Abstracts
{
    public abstract class InGameObject : IDisposable
    {
        private GameObject _gameObject = null;
        private Transform _parent = null;
        public GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = Setup();
                return _gameObject;
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    return _gameObject.name;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set => GameObject.name = value;
        }

        public Vector3 Position
        {
            get => GameObject.transform.localPosition;
            set => GameObject.transform.localPosition = value;
        }

        public Vector3 WorldPosition
        {
            get => GameObject.transform.position;
            set => GameObject.transform.position = value;
        }

        public Quaternion Rotation
        {
            get => GameObject.transform.localRotation;
            set => GameObject.transform.localRotation = value;
        }

        public Vector2 Scale
        {
            get => GameObject.transform.localScale;
            set => GameObject.transform.localScale = value;
        }

        public Vector2 Velocity { get; set; }
        public float Drag { get; set; } = 2f;
        public float Friction { get; set; } = 6f;

        public Vector3 Center => 
            Renderer.bounds.center;

        public Mesh Mesh => 
            MeshFilter.mesh;

        public MeshFilter MeshFilter =>
            GameObject.GetComponent<MeshFilter>();

        public Renderer Renderer => 
            GameObject.GetComponent<Renderer>();

        public bool Enabled
        {
            get => GameObject != null && GameObject.activeSelf;
            set => GameObject.SetActive(value);
        }

        public bool IsGrounded { get; protected set; }

        public float Gravity { get; set; } = 0f;
        public bool CollisionEnabled { get; set; } = true;

        public InGameObject(Transform parent = null)
        {
            _parent = parent;
        }

        public InGameObject(string name, Transform parent = null) : this(parent)
        {
            Name = name;
        }

        private GameObject Setup()
        {
            var obj = new GameObject();
            obj.AddComponent<MeshRenderer>();
            obj.AddComponent<MeshFilter>();

            obj.GetComponent<Renderer>().material.color = Color.white;

            Build(obj);

            obj.name = Guid.NewGuid().ToString();
            if (_parent != null)
                obj.transform.SetParent(_parent);

            obj.AddComponent<BehaviourManager>();
            obj.GetComponent<BehaviourManager>().OnStart += (object sender, EventArgs e) => OnStart();
            obj.GetComponent<BehaviourManager>().OnUpdate += (object sender, EventArgs e) => OnLocalUpdate();
            obj.GetComponent<BehaviourManager>().OnCollisionOn += (object sender, CollisionEventArgs e) => OnCollisionEnter(e.Collision);
            obj.GetComponent<BehaviourManager>().OnCollisionOff += (object sender, CollisionEventArgs e) => OnCollisionExit(e.Collision);
            obj.GetComponent<BehaviourManager>().OnTriggerOn += (object sender, ColliderEventArgs e) => OnTriggerEnter(e.Collider);
            obj.GetComponent<BehaviourManager>().OnTriggerOff += (object sender, ColliderEventArgs e) => OnTriggerExit(e.Collider);
            return obj;
        }

        private Vector3 _previousPosition = new Vector3();

        private void OnLocalUpdate()
        {
            OnUpdate();

            if (Gravity != 0f)
            {
                // add gravity
                //Velocity += Vector3.ClampMagnitude(new Vector3(0, -Gravity, 0) * Time.deltaTime, 3);
            }

            // add drag/friction
            Velocity *= Mathf.Clamp01(1.0f - ((IsGrounded ? Friction : Drag) * Time.deltaTime));

            (Position, Velocity) = CalculateMovement();

            IsGrounded = Position.y == _previousPosition.y;
            _previousPosition = new Vector3(Position.x, Position.y, Position.z);
        }

        protected abstract void Build(GameObject obj);
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract (Vector3 position, Vector3 velocity) CalculateMovement();
        protected virtual void OnCollisionEnter(Collision collision) { }
        protected virtual void OnCollisionExit(Collision collision) { }
        protected virtual void OnTriggerEnter(Collider collider) { }
        protected virtual void OnTriggerExit(Collider collider) { }
        protected virtual void OnObjectDispose() { }

        public void SetParent(Transform parent) =>
            GameObject.transform.parent = parent;

        public void SetParent(InGameObject obj) =>
            SetParent(obj.GameObject.transform);

        public void SetTexture(Texture2D texture) =>
            Renderer.material.SetTexture("_MainTex", texture);

        public void Dispose() => Dispose(true);

        public void Dispose(bool triggerEvent)
        {
            OnObjectDispose();
            string name = Name;
            if (_gameObject != null)
            {
                GameObject.Destroy(_gameObject);
                _gameObject = null;
            }
            if (triggerEvent)
                OnDispose?.Invoke(this, new InGameObjectEventArgs(name));
        }

        public event EventHandler<InGameObjectEventArgs> OnDispose;
    }

    public class InGameObjectEventArgs
    {
        public string Name { get; }

        public InGameObjectEventArgs(string name)
        {
            Name = name;
        }
    }
}
