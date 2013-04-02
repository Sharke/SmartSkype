<?php 
$path = '.'; 
foreach (new DirectoryIterator($path) as $file)
{
    if($file->isDot()) continue;

    if($file->isDir())
    {
         unlink($file->getFilename() . '/online.txt');
    }
}
?>