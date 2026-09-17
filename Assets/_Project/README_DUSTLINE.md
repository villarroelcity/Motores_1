# DUSTLINE — Prototipo Parcial 1

Objetivo técnico de esta rama: transformar el prototipo anterior de Núcleo Cero en la primera versión jugable de DUSTLINE sin perder el trabajo histórico.

## Loop del prototipo

1. Explorar el taller a pie.
2. Inspeccionar el vehículo: necesita una batería.
3. Activar el generador mediante Raycast + `IInteractable`.
4. El generador dispara un `UnityEvent` e instancia `Battery.prefab`.
5. Recoger la batería.
6. Instalarla en el vehículo.
7. El vehículo dispara un `UnityEvent` y abre el portón.
8. Cruzar el trigger de salida.

## Requisitos del Parcial 1 cubiertos

- Tercera persona + `CharacterController`.
- New Input System.
- Cámara funcional.
- Greybox.
- Rigidbody + Colliders + `OnCollisionEnter`.
- Trigger de finalización.
- Prefab + `Instantiate` + `Destroy` controlado.
- Raycast + tecla de interacción.
- `IInteractable`.
- `UnityEvent`.
- Mecánica principal vinculada con el High Concept.

## Generar la escena

Abrir el proyecto en Unity 6000.3.5f2 y ejecutar:

`DUSTLINE > Build or Rebuild Prototype Scene`

Se genera:

`Assets/_Project/Scenes/Dustline_Prototype.unity`

La escena anterior `Assets/Scenes/NucleoCero.unity` se conserva intacta como referencia histórica.
