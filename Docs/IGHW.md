# IGHW

The IGHW file is the main file format used in the PS3 Insomniac Games. The typical file extension is .dat, but check the magic number if in doubt.

## IGHW Header

The IGHW header (0x20 bytes long) goes as such

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  u32 | magic number (0x49474857 or "IGHW")
|  0004  |  u16 | major version number (unconfirmed)
|  0006  |  u16 | minor version number (unconfirmed)
|  0008  |  u32 | number of [sections](#ighw-section-header)
|  000C  |  u32 | length of header (length of this + length of all section headers)
|  0010  |  u32 | length of file

## IGHW Section Header

All IGHWs are made up of lots of sections. the section headers start at 0x20 and each one is 0x10 bytes long.

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  u32 | section identifier
|  0004  |  u32 | offset
|  0008  |  u8  | flags (unconfirmed) (if 0x10, then there are multiple items. if 0x00, there is only one)
|  0009  |  u24 | number of items
|  000C  |  u32 | size of one item

## IGHW Pointers

Many IGHW sections consist purely of these pointers. These mainly point to other files (see assetlookup.md), but can point to other things.

| Offset | Type | Description
|--------|------|-------------------------------------
|  0000  |  u64 | [tuid](#whats-a-tuid)
|  0008  |  u32 | offset
|  000C  |  u32 | misc (usually length)

## What's a tuid?

A tuid is a unique identifier, similar to windows' GUIDs