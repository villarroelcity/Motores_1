# DUSTLINE — Parcial 1 / Estación 01 V2

Proyecto de **Motores de Desarrollo 1 — Parcial 1**.

- Unity: **6000.3.5f2**
- Render Pipeline: **URP**
- Input: **New Input System**
- Rama de trabajo: **`dustline-parcial1`**

## Escena principal

`Assets/Scenes/Dustline_Prototype.unity`

La escena vieja de Núcleo Cero fue eliminada. DUSTLINE mantiene la estructura original del proyecto (`Assets/Scenes`, `Assets/Scripts`, `Assets/Prefabs`, etc.).

## Estación 01 V2

La versión V2 reemplaza el rectángulo de prueba por una estación de servicio/taller con sectores reconocibles:

- acceso exterior y playa de servicio;
- taller principal con bahía de reparación;
- vehículo objetivo construido como placeholder legible;
- patio lateral del generador;
- depósito `PARTS`;
- estanterías, bancos de trabajo y señalización;
- cajas y barriles físicos;
- portón de salida animado;
- tramo de Ruta 17 como cierre y teaser del Parcial 2;
- iluminación del taller que se enciende al restaurar energía;
- faros y estado visual del vehículo al instalar la batería;
- HUD con objetivo, prompt de interacción y pantalla final.

## Loop jugable

`inspeccionar vehículo → activar generador → encender taller → generar batería → recuperar batería → instalarla → encender vehículo → abrir portón → alcanzar Ruta 17`

## Controles

- **WASD / flechas:** mover.
- **Mouse:** cámara.
- **E:** interactuar.
- **Esc:** liberar cursor.
- **Clic izquierdo:** capturar cursor nuevamente.

## Requisitos técnicos demostrados

- tercera persona con `CharacterController`;
- New Input System;
- cámara orbital con detección de obstáculos;
- GameObjects y componentes;
- greybox navegable con sectores y puntos de interés;
- Rigidbody y Colliders;
- `OnCollisionEnter` en cajas/barriles;
- `OnTriggerEnter` en salida;
- prefab `Battery.prefab`;
- `Instantiate` en PARTS y `Destroy` al recoger;
- Raycast + tecla E;
- interfaz `IInteractable`;
- `UnityEvent` real: generador → luces/spawner y vehículo → feedback/portón;
- objetivo y condición de finalización.

## Builder de seguridad

`Assets/Editor/DustlineSceneBuilderV2.cs`

Al abrir por primera vez esta versión, Unity verifica que la escena tenga el root `DUSTLINE_LEVEL_V2`. Si detecta la escena anterior, la reconstruye automáticamente y abre la nueva Estación 01.

También puede ejecutarse manualmente desde:

`DUSTLINE > Rebuild Estación 01 (V2)`

Una vez generado el root V2, no vuelve a reconstruir la escena automáticamente, por lo que se puede editar normalmente.

## Documentación

En `Documentacion/` se incluye el High Concept, GDD, testing, checklist y `Dustline_Estacion01_Blockout.png`.
