This app takes the output of the LIE.exe, an export of a current library as tags in a csv file and produces
files importable in neo4j of two types

* fact or nodes files:

- a file for all the albums with the following format:
	albumID:ID(Album),name,location
- a file for all the tracks with the following format:
	trackID:ID(Track),name,file
- a file for all the artists including featured artists, album artists and composers with the following format:
	artistID:ID(Artist),name,:LABEL

* relationship files:
- a  file containing all the links between tracks and albums in the following format
	:START_ID(Track),track_no,:END_ID(Album)
- a file containing all the links between artists and bands and tracks in the format
	:START_ID(Artist),year,:END_ID(Track)
- a file containing all the links between the artists or bands featured on tracks and tracks in the format
	:START_ID(Artist),year,:END_ID(Track)
- a file containing all the links between composers and the tracks in the following format
	:START_ID(Artist),:END_ID(Track)

It is ussually followed by AC2.exe