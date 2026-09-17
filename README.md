# DUSTLINE

Proyecto de **Motores de Desarrollo 1 — Parcial 1**.

- Unity: **6000.3.5f2**
- Render Pipeline: **URP**
- Input: **New Input System**
- Rama de trabajo: **`dustline-parcial1`**

## Escena principal

Abrir directamente:

`Assets/Scenes/Dustline_Prototype.unity`

La escena vieja **NucleoCero** y la escena **SampleScene** fueron eliminadas para dejar el proyecto limpio.

## Loop del Parcial 1

`explorar → activar generador → instanciar batería → recoger batería → instalarla en el vehículo → abrir portón → cruzar salida`

La conducción avanzada queda planificada para el Segundo Parcial.

## Controles

- **WASD / flechas:** mover.
- **Mouse:** cámara.
- **E:** interactuar.
- **Esc:** liberar cursor.
- **Clic izquierdo:** capturar cursor nuevamente.

## Requisitos técnicos cubiertos

- Personaje en tercera persona con `CharacterController`.
- New Input System.
- Cámara tercera persona.
- Greybox del taller.
- Rigidbody y Colliders.
- `OnCollisionEnter` en objeto físico.
- Trigger de finalización.
- Prefab `Battery.prefab`.
- `Instantiate` y `Destroy` controlado.
- Raycast de interacción.
- `IInteractable`.
- `UnityEvent` en interactuables.
- Mecánica principal vinculada con el High Concept.

## Estructura del proyecto

- `Assets/Scenes/` — escenas del juego.
- `Assets/Scripts/` — scripts del juego.
- `Assets/Prefabs/` — prefabs reutilizables.
- `Assets/Materials/` — materiales del proyecto.
- `Assets/Settings/` — configuración URP.
- `Assets/InputSystem_Actions.inputactions` — acciones del New Input System.
- `Documentacion/` — High Concept, GDD, testing y material de entrega.

No se utiliza una carpeta paralela `_Project`: DUSTLINE respeta la estructura original del proyecto.
