# DUSTLINE

Proyecto de **Motores de Desarrollo 1 — Parcial 1**.

- Unity: **6000.3.5f2**
- Render Pipeline: **URP**
- Input: **New Input System**
- Rama de trabajo: **`dustline-parcial1`**

## Escena lista

La escena del prototipo ya está creada y configurada como primera escena de Build Settings:

`Assets/_Project/Scenes/Dustline_Prototype.unity`

La escena histórica de Núcleo Cero se conserva, deshabilitada en Build Settings:

`Assets/Scenes/NucleoCero.unity`

## Loop del Parcial 1

`explorar → activar generador → instanciar batería → recoger batería → instalarla en el vehículo → abrir portón → cruzar salida`

La conducción avanzada queda planificada para la siguiente etapa del proyecto.

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
- `UnityEvent` disponible en los interactuables.
- Mecánica principal vinculada con el High Concept.

## Estructura principal

- `Assets/_Project/Scenes/` — escena actual.
- `Assets/_Project/Prefabs/` — batería.
- `Assets/_Project/Scripts/Gameplay/` — lógica de DUSTLINE.
- `Assets/_Project/Editor/` — builder opcional para reconstruir el greybox.
- `Documentacion/` — High Concept y material de entrega.

## Builder opcional

La escena ya viene incluida. Si se necesita reconstruir desde cero dentro de Unity:

`DUSTLINE > Build or Rebuild Prototype Scene`

No es necesario ejecutar este menú para comenzar a trabajar.
