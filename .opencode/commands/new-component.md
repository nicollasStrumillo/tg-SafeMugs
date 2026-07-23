---
description: Create the 3 standard files for a angular componment: .ts, .html and .scss
agent: build
model: opencode/deepseek-v4-flash-free
---

Create in the folder provided in the argument $1 the 3 standart files that make a regular angular component utilizing the own name of the folder provided in the argument $1:
$1.html - completely empty
$1 - completely empty
$1.ts:
Write a basic scaffold based on the file @frontend\src\app\pages\score-board\score-board.ts. This scaffold should let me immediately write the relevant business code; create the "@component" part; import obvious libraries like "{ ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core'; and etc."