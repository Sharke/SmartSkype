<?php
$user = $_GET['user'];
$userlist = "users.txt";
$userlistcontents = file_get_contents(users.txt);
$openlist = fopen($userlist, 'w') or die ("");
fwrite($openlist, $userlistcontents . $user . "/n";
fclose($openlist);
?>
