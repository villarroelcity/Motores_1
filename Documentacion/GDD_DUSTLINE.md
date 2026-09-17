# GDD — DUSTLINE

## Proyecto

**DUSTLINE**  
Motores de Desarrollo 1 — Unity  
Versión del documento: Parcial 1 / Prototipo inicial

**Integrantes:** [COMPLETAR]

## Género

Aventura / exploración en tercera persona, con conducción planificada para la siguiente etapa.

## High Concept

DUSTLINE es una aventura de exploración y conducción en tercera persona donde un mensajero mecánico recupera vehículos abandonados para reconstruir las rutas entre asentamientos aislados.

## Objetivo principal

Recuperar vehículos e infraestructura para restablecer las conexiones entre zonas aisladas.

## Mecánica central

**Recuperación de vehículos:** explorar una instalación, identificar qué necesita un vehículo y realizar las acciones necesarias para dejarlo operativo.

## Mecánicas secundarias previstas

### Implementadas en Parcial 1

- Movimiento en tercera persona.
- Cámara de seguimiento.
- Exploración de un greybox.
- Interacción mediante Raycast.
- Activación del generador.
- Instanciación de batería.
- Recogida de batería.
- Instalación en vehículo.
- Apertura de portón.
- Físicas, colisiones y trigger final.

### Planificadas para etapas posteriores

- Entrada y salida de vehículos.
- Conducción con sistema de física vehicular.
- Recorrido exterior entre zonas.
- Nuevos puntos de reparación.
- Diferentes vehículos/terrenos si el alcance lo permite.

## Gameplay actual

1. El jugador llega a pie desde la ruta y entra a la Estación de Servicio 01.
2. Explora el taller y puede inspeccionar el vehículo inmovilizado en la bahía.
3. El vehículo informa que necesita una batería.
4. El jugador sale por el acceso lateral hacia el patio del generador.
5. Al activar el generador, mediante `UnityEvent`, se enciende la iluminación del taller y se habilita el módulo en PARTS.
6. `BatterySpawner` instancia `Battery.prefab` en el depósito de repuestos.
7. El jugador recupera la batería y regresa al vehículo.
8. Al instalarla, se activan los faros/estado visual del vehículo y comienza a abrirse el portón de ruta.
9. El jugador atraviesa el portón y alcanza Ruta 17, activando el trigger de finalización.

## Core Gameplay Loop completo previsto

`explorar → diagnosticar → recuperar/reparar → conducir → llegar a nueva zona → repetir`

## Diseño preliminar del nivel

Ver `Documentacion/Dustline_Estacion01_Blockout.png`.

La escena se divide en acceso exterior, taller, bahía del vehículo, patio del generador, depósito PARTS y salida hacia Ruta 17. Esta separación busca que el recorrido sea legible y que cada sistema técnico se demuestre dentro de una función coherente del nivel.

## Estado actual de desarrollo

### Funcionalidades implementadas

- Proyecto configurado en Unity 6000.3.5f2.
- URP.
- New Input System.
- `CharacterController`.
- Cámara tercera persona.
- Escena `Dustline_Prototype`.
- Greybox V2 navegable con exterior, taller, patio de generador, depósito PARTS y salida a ruta.
- `IInteractable`.
- Raycast.
- `GeneratorInteractable` conectado por `UnityEvent` a iluminación y spawner.
- `Battery.prefab`.
- `Instantiate`.
- Rigidbody/Collider.
- `OnCollisionEnter`.
- Recogida y `Destroy` controlado.
- VehicleInteractable.
- `GarageDoor` con apertura progresiva.
- `OnTriggerEnter` de finalización.

### Pendiente de validación manual en Unity

- Confirmar que no existan errores de compilación/importación.
- Ajustar posiciones o cámara si hiciera falta.
- Completar testing real.
- Generar build.
- Publicar en itch.io.

### Funcionalidades futuras

- Conducción.
- Integración con NWH Vehicle Physics 2 en la etapa correspondiente.
- Ruta exterior.
- Segunda zona.
- Presentación visual y sonora más avanzada.

## Alcance

El Parcial 1 se concentra en demostrar los sistemas técnicos de las clases 1 a 7 mediante un prototipo pequeño y estable. La conducción y el vertical slice se dejan para el desarrollo posterior.
