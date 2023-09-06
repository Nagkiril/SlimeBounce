# SlimeBounce

This repository will contain major complete versions of my personal Android game.

I've made it because I wanted to showcase what I can do - in a way that also results in something I would call a fun game, made pretty much entirely by me (bar some often-used packages, of course). Despite its age, it still feels somewhat like a prototype, but I find that I achieved what I wanted here, and thus I move on. More on this in **State of the Game** below.

Each individual push is made in a private repository elsewhere, thus updates here are expected to be scarce. Check a dev branch for the project itself and changelog past previous releases.

# Gameplay Overview

A Slime ball is falling down a road, and Player must eliminate it via one of the following methods:
- Dragging to the side of the screen, over a dropout zone that have individual cooldowns after use.
- Exploiting properties of other slimes (every slime type has a unique twist in their behaviour).
- Utilizing Ability buttons on the bottom part of the screen, when they're unlocked.

A slime that reaches the end of the road deducts 1 life point. Reaching 0 life points will lose the level, and eliminating all slimes on the level is a victory.

After completing each level, Player receives Experience and Gold:
- Experience contributes towards the Player Level, upon levelling up, Player unlocks additional Slime Types, starts receiving more Gold, but slimes may spawn and move faster.
- Gold is used to purchase Passive upgrades and Abilities in the Shop.

Progression is, of course, saved.

# State of the Project

There are no plans to make any additional contributions to the project, with branch dev_1.0.0 containt the complete implementation of my Game Design Document.

Some things are left in imperfect form, such as:
- Game likely contains bugs, however nothing significant is registered by me at the moment.
- Floor and some visuals (such as floor buttons or the logo) are looking off, however for my skillset I find them rather satisfactory: I consider myself an all-rounder, but I still lean away from art and more towards programming.
- Lategame content and game balance could use more work, but I still find it quite fun - and a good game could very often use more content anyway.
- In terms of showcasing my technical knowledge, project uses most of what I know (at the time of the 1.0.0 release, at least), but not all - I would say that quality is around average in relation to what I'm capable of. I think it's a safe and fair bet for a showcase project like this.
- There are some additional optimizations that project is missing, however, on my test devices it runs on solid 60 FPS, and overall I find it satisfactory on that front too.
