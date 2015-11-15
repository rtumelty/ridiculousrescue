using UnityEngine;
using UnityEditor;
using System.Collections;

public class ImportControl : AssetPostprocessor
{
	[MenuItem("Import/Reimport sounds")]
	public static void reimportAllSounds ()
	{
		AssetDatabase.ImportAsset ( "Assets/Resources/Sounds", ImportAssetOptions.ImportRecursive );
	}
	
	private void OnPreprocessAudio ()
	{
		AudioImporter importer = ( AudioImporter ) assetImporter;
		importer.format = AudioImporterFormat.Compressed;
		importer.forceToMono = true;
		importer.compressionBitrate = 64000;
		importer.loadType = AudioImporterLoadType.CompressedInMemory;
	}
}