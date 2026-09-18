using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static PlayerAuthoring;
using static UnityEngine.EventSystems.EventTrigger;

[UpdateBefore(typeof(PlayerAttackSystem))]
public partial class PlayerInputSystem : SystemBase
{
    private PlayerInput _input;
    protected override void OnCreate()
    {
        _input = new PlayerInput();
        _input.Enable();
    }
    protected override void OnUpdate()
    {
        var movementInput = (float2)_input.Player.Move.ReadValue<Vector2>();
        var attackInput = _input.Player.Attack.triggered;
        //var lookInput = (float2)_input.Player.Look.ReadValue<Vector2>();


        foreach (var (direction, commands, player) in SystemAPI.Query<RefRW<CharacterMoveDirection>, RefRW<PlayerCommands>>().WithAll<PlayerTag>().WithEntityAccess())
        {
            direction.ValueRW.Value = movementInput;
            commands.ValueRW.Attack = attackInput;
            //commands.ValueRW.PointerWorldPosition = lookInput;


            Vector2 screenPosition = _input.Player.Look.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            Plane groundPlane = new Plane(Vector3.up, 0f);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);

                commands.ValueRW.PointerWorldPosition = worldPosition;
            }
        }
    }
}
