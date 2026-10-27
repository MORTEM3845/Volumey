# Issue 105 mock session verification

No real audio devices or sessions were accessed. The state uses two mocked `AudioSessionModel` instances owned by one `AudioProcessModel`.

| Action | App group value | Session 1 | Session 2 |
|---|---:|---:|---:|
| Initial | 50 | 50 | 50 |
| Set app slider to 70 | 70 | 70 | 70 |
| Set Session 2 to 35 | 70 | 70 | 35 |

Expected defect illustration before the fix: moving the app slider to 70 changed only Session 1 (Session 2 remained 50). After the fix, app slider writes every session; a child slider updates only its own session and does not move the app slider.

Automated evidence: `AudioProcessModelTests` passed 23/23, including the two-session group and independent child-volume checks.
