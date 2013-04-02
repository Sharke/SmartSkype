<?php
error_reporting(0);
$user      = $_GET['user'];
$name      = $_GET['name'];
$path      = base64_decode($user) . "/" . $name . "/";
$global    = base64_decode($user) . "/" . "global" . "/";
$globaluse = file_get_contents($global . "/" . "global.txt");

if (stripos($globaluse, $name) !== False)
 {

    foreach (new DirectoryIterator($global) as $file) {
        if ($file->isDot())
            continue;
        
        if ($file->isFile()) {
if($file->getFilename() !== "global.txt")
            {

            $data = file_get_contents($global . $file->getFilename());

            if ($data != "") {
                echo $data;
                $globaluse = str_replace($name, "", $globaluse);
                $myFile    = $global . "/" . "global.txt";
                $fh = fopen($myFile, 'w') or die("");
                fwrite($fh, $globaluse);
                fclose($fh);
                exit;
            }
            	}
        }
    }
}

foreach (new DirectoryIterator($path) as $file) {
    if ($file->isDot())
        continue;
    
    if ($file->isFile()) {
        $data = file_get_contents($path . $file->getFilename());
        if ($data != "") {
            echo $data;
            unlink($path . $file->getFilename());
            exit;
        }
    }
}
?>