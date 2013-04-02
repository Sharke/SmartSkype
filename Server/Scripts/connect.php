<?php
$name = $_GET['name'];
$user = $_GET['user'];
$data = file_get_contents(base64_decode($user) . "/" . "online.txt");
if (!file_exists(base64_decode($user))) {
    mkdir(base64_decode($user), 0777);
    
}
if (!file_exists(base64_decode($user) . "/" . $name)) {
    mkdir(base64_decode($user) . "/" . $name, 0777);
} else {
}

if (strpos($data, $name) !== TRUE) {
    $myFile = base64_decode($user) . "/" . "online.txt";
    $fh = fopen($myFile, 'w') or die("");
    $stringData = $data . $name . "\n";
    fwrite($fh, $stringData);
    fclose($fh);
    exit;
} else {
}

?>