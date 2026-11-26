# Changelog

## Unreleased

- If a game update breaks part of the mod, only that part turns off and the rest keeps working: if holding you still on a roof stops working, you still won't slide off. The log says which part is off, in one line. If the part that stops you sliding breaks, the whole mod turns off cleanly instead of throwing errors.
- New `DisabledFeatures` setting under `[Debug]`, for testing: turns parts of the mod off by name.

## 1.0.0

- First stable release.
- You now also keep your footing on roofs that are already in the world: the steep roofs of fuling huts and towers, and the arched roofs of the Ashlands ruins. Before, only roofs built by players counted.

## 0.1.0

- Players no longer slide off steep roofs. Vanilla slides you on anything steeper than 38 degrees, so 45 and 67 degree roofs now hold you like 26 degree ones.
- Standing still on a roof holds you in place, even on the steepest 67 degree pieces. Like standing on a ship in vanilla, the spot where you stopped is remembered and you're kept there until you move. Your friction is also raised while you stand still, and walking feels the same as vanilla.
