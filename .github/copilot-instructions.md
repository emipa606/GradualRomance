# GitHub Copilot Instructions for Gradual Romance (Continued)

## Mod Overview and Purpose

**Gradual Romance (Continued)** is a comprehensive overhaul of the relationship dynamics in RimWorld, originally developed by Nationalitys. This mod completely reworks how relationships form, progress, and are perceived in the game. The emphasis is on a gradual process where relationships evolve over time with various stages and interactions, unlike the default binary system in Vanilla RimWorld.

### Key Features and Systems

1. **New Interactions**: Introduces new social interactions such as "Flirt" and "AskToMoveIn", which allow pawns to engage in nuanced romantic dynamics based on their personalities.

2. **New Romance Stages**: Adds several stages of romance progression:
   - **Lovebuddies**: Physically affectionate but non-romantic.
   - **Sweethearts**: Emotionally attached but not yet ready for deeper intimacy.
   - **Paramour**: Engaged in both romantic and physical interactions, with no exclusivity.
   - **Boyfriend/Girlfriend (Lovefriend)**: Serious commitment, sharing aspects of a lover's relationship but without cohabitation.

3. **Attraction Calculation Overhaul**: Significant alterations to how attraction is computed, introducing additional factors like personality traits, wealth, popularity, and more. This system is also configurable via XML, allowing players to modify attraction dynamics.

4. **Alien Integration**: Attempts to intelligently integrate humanoid aliens into the attraction and relationship systems using inferred defaults.

5. **Harmony Patches**: Modifies core game functions through Harmony for compatibility and enhancement of interactions provided by Gradual Romance.

6. **Compatibility Requirements**: Requires HugsLib and Psychology mods and should load after them. Incompatible with most relationship-oriented mods, especially Rational Romance.

---

## Coding Patterns and Conventions

- **Class Inheritance**: The `AttractionCalculator` class serves as a base for various specific attraction calculators (e.g., `AttractionCalculator_Ability`, `AttractionCalculator_Beauty`). This allows for specialized logic handling under a unified framework.

- **Separation of Concerns**: Each feature or system aspect, like flirt reactions or attraction calculations, is encapsulated within its respective class, often extending base classes or using inheritance for modularity.

- **Static Utilities**: Utilizes static classes for utility functions (`GRThoughtUtility`, `AttractionUtility`) ensuring ease of access across different parts of the mod.

- **DefModExtensions**: Utilizes `DefModExtension` for easy extensions of game definitions (Defs), allowing additional data fields for mod-specific functionalities.

---

## XML Integration

The mod leverages XML for defining configurable aspects of the attraction system such as factors influencing attraction and their weight values. This is managed through Defs like `AttractionFactorDef` and `AttractionFactorCategoryDef`.

- XML configurations allow for the introduction of custom attributes and modification of existing ones, providing flexibility in gameplay adjustments without altering the core mod C# code.

---

## Harmony Patching

Gradual Romance extensively uses Harmony to patch the game's methods, ensuring seamless integration without altering the original game files. Some key patches include:

- **`GRExistingLovePartnerPatch`**: Ensures the mod’s relationship systems co-exist with existing partner statuses in vanilla RimWorld.
- **`GRSecondaryRomanceChanceFactorPatch`**: Adjusts the secondary factors impacting romance attempt chances.
- **`GRPawnGeneratorPatch`**: Modifies pawn generation to accommodate new romance and attraction factors.

---

## Suggestions for Copilot

- **Code Navigation**: Copilot can assist with navigating between the extensive network of attraction calculator classes and their specialized logic. Ensure you refer to the base class for understanding shared functionalities.

- **Code Completion**: Utilize Copilot for quickly writing repetitive methods with similar logic structures across different attraction calculators or flirt reaction workers.

- **Error Handling**: Consider adding additional validation logic for XML data parsing with Copilot suggestions to ensure robustness.

- **Optimization Alerts**: Use Copilot to identify potential performance bottlenecks, particularly related to loops that calculate attraction scores across multiple factors.

- **Patch Integration**: When creating new features, ensure all necessary Harmony patches are documented. Copilot can assist in drafting consistent comments explaining the purpose of each patch.

This file will serve as a guideline to enhance and maintain the Gradual Romance mod, ensuring future developments adhere to the established structures and processes.
