# DUSTLINE

Proyecto de **Motores de Desarrollo 1 — Parcial 1**.

Unity: **6000.3.5f2**, URP y New Input System.

Esta rama transforma el prototipo anterior **Núcleo Cero** en el prototipo inicial de DUSTLINE. El objetivo es conservar los sistemas técnicos que ya funcionaban y reutilizarlos dentro de una propuesta con continuidad hacia el segundo parcial.

## Concepto

DUSTLINE es una aventura de exploración y conducción en tercera persona donde un mensajero mecánico recupera vehículos abandonados para reconstruir las rutas entre asentamientos aislados.

### Alcance del Parcial 1

En esta primera versión el jugador todavía no conduce. El loop jugable es:

`explorar → restaurar energía → recuperar batería → preparar vehículo → abrir portón → finalizar`

La conducción avanzada queda planificada para la siguiente etapa del proyecto.

## Estado de implementación

Se conserva el prototipo histórico en `Assets/Scenes/NucleoCero.unity`.

La nueva implementación está en `Assets/_Project/` e incluye:

- `DustlineProgress`: estado mínimo de la secuencia.
- `DustlinePlayerInteraction`: Raycast + acción `Player/Interact` del New Input System.
- `GeneratorInteractable`: interacción mediante `IInteractable` y `UnityEvent`.
- `BatterySpawner`: `Instantiate` del prefab de batería.
- `BatteryPickup`: recogida y `Destroy` controlado.
- `PlayerCargo`: estado simple de una batería, sin inventario complejo.
- `VehicleInteractable`: instalación de batería y recuperación del vehículo.
- `GarageDoor`: apertura observable del portón.
- `DustlineFinishTrigger`: finalización mediante Trigger.
- `CollisionFeedback`: ejemplo visible en consola de `OnCollisionEnter`.
- `DustlineSceneBuilder`: genera el greybox completo sin modificar la escena vieja.

## Crear la nueva escena

1. Abrir con Unity **6000.3.5f2**.
2. Esperar a que termine de compilar.
3. Ejecutar `DUSTLINE > Build or Rebuild Prototype Scene`.
4. Abrir `Assets/_Project/Scenes/Dustline_Prototype.unity` si Unity no la deja abierta.
5. Presionar Play.

## Controles

- WASD / flechas: movimiento.
- Mouse: cámara.
- E: interactuar mediante la acción `Player/Interact`.
- Esc: liberar cursor.
- Clic izquierdo: capturar cursor nuevamente.

## Git

El repositorio original se conserva y esta migración se trabaja en la rama `dustline-parcial1` para mantener visible la evolución del proyecto.
