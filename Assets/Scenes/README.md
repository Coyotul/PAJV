Setup steps for SampleScene

1) Open Unity, load the project, and open `Assets/Scenes/SampleScene.unity`.
2) In Hierarchy, add an empty `GameManager` object and attach `GameManager`.
   - Assign a `CameraFollow` reference (use Main Camera and add `CameraFollow`).
   - Create an empty `SpawnPoint` object where the player should spawn; drag it to `spawnPoint`.
   - For `playerPrefab`, you can use a primitive:
     a) GameObject -> 3D Object -> Capsule. Add `CharacterController` and `PlayerMovement`.
     b) Drag the Capsule from Hierarchy into `Assets/Prefabs` to create a prefab.
     c) Delete the Capsule from Hierarchy and assign the prefab to `playerPrefab`.
3) Select Main Camera, add `CameraFollow`, and leave default offset.
4) Press Play. Use WASD/Arrows to move; camera follows the spawned player.

Notes
- If you don’t add a `CharacterController` to the player, movement falls back to transform-based.
- You can tweak speeds in `PlayerMovement` and smoothing in `CameraFollow`.



