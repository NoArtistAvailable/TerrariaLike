using System;
using System.Collections;
using System.Collections.Generic;
using elZach.TerrariaLike;
using UnityEngine;
using UnityEngine.InputSystem;

namespace elZach.TerrariaLike
{
    public class PlayerControls : MonoBehaviour
    {
        private Camera _cam;
        private Camera Cam => _cam ??= Camera.main;
        
        public InputActionReference movement;
        public InputActionReference mine;
        public float movementSpeed = 1f;
        public Transform target;

        public event Action<Vector2Int> onMine;

        void OnEnable()
        {
            movement.asset.Enable();
        }

        private void Mine()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane plane = new Plane(Vector3.forward, 0f);
            plane.Raycast(ray, out var enter);
            var pos = ray.GetPoint(enter);
            onMine?.Invoke(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
        }

        void Update()
        {
            var movementInput = movement.action.ReadValue<Vector2>();
            target.Translate(movementInput * movementSpeed * Time.deltaTime);
            if(mine.action.IsPressed()) Mine();
        }
    }
}