# gp_prius.dat

## Identifiers

| Identifier | Description |
|---|---|
| 00011300 | Names for instances.lua |
| 00025005 | Unknown |
| 00025006 | Unknown |
| 00025008 | Some sort of [toc](#table-of-contents) |
| 0002500C |  |
| 00025010 |  |
| 00025014 |  |
| 00025020 |  |
| 00025030 |  |
| 00025048 | [A series of IGHW Prius Elements](#ighw-prius-elements) |
| 00025049 |  |
| 0002504C |  |
| 00025050 |  |
| 00025054 |  |
| 00025058 |  |
| 0002505C |  |
| 00025060 |  |
| 00025064 |  |
| 00025068 |  |
| 0002506C |  |
| 00025070 | A series of IGHW pointers, the identifiers can be found in the comments of instances.lua |
| 00025074 |  |
| 00025078 |  |
| 0002507C |  |
| 00025080 |  |
| 00025084 |  |
| 00025088 |  |
| 0002508C |  |
| 00025090 |  |
| 00025094 |  |

## Table of Contents

| Addr | Size |  Type  | Desc |
|------|------|--------|------|
| 0000 | 0040 | String | Name of toc entry, null terminated string |
| 0040 | 0004 | Int 32 | points to entry in identifier 00025048, 32 bit int |
| 0044 | 0004 | Int 32 | ??, 32 bit int |
| 0048 | 0004 | Int 32 | ??,  |

## IGHW Prius Elements

| Addr | Size |  Type  | Desc |
|------|------|--------|------|
| 0000 | 0002 | Int 16 | index of moby in regions.dat |
| 0002 | 0002 | Int 16 | the first parameter of the moby_handle constructor |
| 0004 | 0004 | Single | Unknown |
| 0008 | 0004 | Single | Unknown |
| 000C | 0004 | Int 32 | Points to an IGHW |
| 0010 | 0004 | Int 32 | Unknown |
| 0014 | 0004 | Single | X coord of the instance |
| 0018 | 0004 | Single | Y coord of the instance |
| 001C | 0004 | Single | Z coord of the instance |
| 0020 | 0004 | Int 32 | X rotation of the instance (euler angles, radians) |
| 0024 | 0004 | Int 32 | Y rotation of the instance (euler angles, radians) |
| 0028 | 0004 | Int 32 | Z rotation of the instance (euler angles, radians) |
| 002C | 0004 | Int 32 | Unknown |
| 0030 | 0004 | Int 32 | Unknown |
| 0034 | 0004 | Int 32 | Unknown |
| 0038 | 0004 | Int 32 | Unknown |
| 003C | 0004 | Int 32 | Unknown |
| 0040 | 0004 | Int 32 | Unknown |
| 0044 | 0004 | Int 32 | Unknown |
| 0048 | 0004 | Int 32 | Unknown |
| 004C | 0004 | Int 32 | Unknown |

## 