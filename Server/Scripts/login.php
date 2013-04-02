<?php
$user = $_GET["user"];
$data = file_get_contents('../ApprovedUsers.txt');

if(strpos($data, $user) !== FALSE)
{
    echo 'True';
}
else
{
    echo 'False';
}

?>
