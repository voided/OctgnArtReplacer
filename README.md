# OctgnArtReplacer
Just a simple tool for replacing OCTGN card art with higher resolution versions from slightlymagic.net

### Usage
`OctgnArtReplacer "<path to source files>" <path to output>" "<set name>"`

Where:
- Path to source files: A flat directory containing card art for the given set in the form `Card Name.size.jpg`.
- Path to output: An output directory that will be created containing art that can be added to OCTGN's image database.
- Set name: The name of the set the source files should be mapped to. This is used for finding the correct cards the art belongs to.
