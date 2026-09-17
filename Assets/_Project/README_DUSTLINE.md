# DUSTLINE — Prototipo Parcial 1

La escena principal ya se encuentra preparada en:

`Assets/_Project/Scenes/Dustline_Prototype.unity`

## Recorrido técnico

1. Explorar el taller a pie.
2. Inspeccionar el vehículo: necesita una batería.
3. Activar el generador mediante Raycast + `IInteractable`.
4. El generador instancia `Battery.prefab`.
5. Recoger la batería; el prefab se destruye de forma controlada.
6. Instalarla en el vehículo.
7. Se abre el portón del taller.
8. Cruzar `FinishTrigger`.

## Componentes demostrables

- `PlayerMovement` — CharacterController, gravedad y movimiento.
- `ThirdPersonCamera` — seguimiento y control por mouse.
- `DustlinePlayerInteraction` — Raycast + New Input System.
- `GeneratorInteractable` — interfaz + evento + activación.
- `BatterySpawner` — Instantiate.
- `BatteryPickup` — interacción + Destroy.
- `PlayerCargo` — estado mínimo; no es un inventario complejo.
- `VehicleInteractable` — instalación de batería.
- `GarageDoor` — respuesta observable.
- `CollisionFeedback` — OnCollisionEnter.
- `DustlineFinishTrigger` — OnTriggerEnter y condición final.

La escena vieja `Assets/Scenes/NucleoCero.unity` se mantiene como referencia histórica.
