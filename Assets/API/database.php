<?php

$hostname = '217.107.34.98';
$username = 'api';
$password = 'b@F7@nE79P';



try{
    $users_db = new PDO('mysql:host=' . $hostname . ';dbname=' . 'voplik_games', $username, $password);
}
catch(PDOException $e){
    echo 'Error: ' . $e->getMessage();
}

?>