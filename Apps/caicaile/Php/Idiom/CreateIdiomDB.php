<?php
header("Content-type: text/html; charset=utf-8");

include('ParseIdiomItem.php');
include_once('IdiomItemInfo.php');

class CreateIdiomDB //extends Thread
{

    public $url;
    public $id;
    public $channel;
    public $listItem;
    public  $page_total = 10; //10
    public $htmlItem;

    public $WEB_HOME = "https://chengyu.911cha.com";
    public $ROOT_SAVE_DIR = "Data";

    public function DoCreate()
    {
        $this->listItem =   array();
        $this->htmlItem =   new simple_html_dom();
        //https://chengyu.911cha.com/pinyin_a.html
        $save_dir = $this->ROOT_SAVE_DIR . "/All_Idiom";
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        $this->PaserSort();
        //save
        $savefilepath = $save_dir . "/idiom_list.json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

        $count = count($this->listItem);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listItem),
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }


    function PaserSort()
    {
        //https://chengyu.911cha.com/pinyin_a.html

        $html = get_html("https://chengyu.911cha.com/pinyin_a.html");
        if (!$html) {
            echo "open html fail\n";
            return;
        }

        //<span class="choice">
        $div_main = $html->find('span[class=choice]', 0);
        if (!$div_main) {
            echo "PaserSortByLetter find div_main fail\n";
            return;
        }

        $arry_a = $div_main->find('a');
        $count = count($arry_a);
        echo "arry_a count =" . $count . "\n";

        //https://chengyu.911cha.com/pinyin_b.html
        foreach ($arry_a as $a) {
            $url = $this->WEB_HOME . substr($a->href, 1);
            $title =  $a->plaintext;
            if (($title != "Y") && ($title != "Z")) {
                continue;
            }
            echo "PaserSort url:  " . $url . " title:" . $title . "\n";
            $total_page = $this->GetPageCount($url);
            for ($i = 0; $i < $total_page; $i++) {
                $this->PaserIdiomList($this->GetPageUrl($title, $i));
            }
        }
    }
    function GetPageUrl($letter, $page)
    {
        return "https://chengyu.911cha.com/pinyin_" . $letter .  "_p" . $page . ".html";
    }
    function GetPageCount($url)
    {
        $html = get_html($url);
        $ret = 0;
        if (!$html) {
            echo "GetPageCount open html fail\n";
            return 0;
        }
        $div_main = $html->find('div[class=gclear pp bt center f14]', 0);
        if (!$div_main) {
            echo "GetPageCount find div_main fail\n";
            return 0;
        }
        $arry_a = $div_main->find('a');
        $ret = count($arry_a) - 4;
        return $ret;
    }

    //https://chengyu.911cha.com/pinyin_a.html
    function PaserIdiomList($url)
    {
        $html = get_html($url);
        if (!$html) {
            echo "PaserIdiomList open html fail\n";
            return;
        }
        //table : id table1
        $div_main = $html->find('ul[class=l5 center f14]', 0);
        if (!$div_main) {
            echo "PaserIdiomList find div_main fail\n";
            return;
        }
        //  echo "div_main =\n" .  $div_main . innertext;

        $arry_a = $div_main->find('a');
        $count = count($arry_a);
        echo "arry_a count =" . $count . "\n";
        foreach ($arry_a as $a) {
            $url = $this->WEB_HOME . substr($a->href, 1);

            $parse = new ParseIdiomItem();
            $info = $parse->ParseIdiomItemInfo($url);

            $title =  $a->plaintext;
            echo "url:  " . $url . " title:" . $title . "\n";

            $element = array(
                'url' => $url, //谜面
                'title' => $title,
                'pinyin' => $info->pinyin,
                'translation' => $info->translation,
                'album' => $info->album,
            );
            array_push($this->listItem, $element);
            $this->SaveOneIdiom($info);
        }

        $this->Save();
    }

    function  SaveOneIdiom($info)
    {
        $save_dir = $this->ROOT_SAVE_DIR . "/All_Idiom";
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        //save
        $savefilepath = $save_dir . "/" . $info->title . ".json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        } {

            $element = array(
                'title' => $info->title,
                'pinyin' => $info->pinyin,
                'translation' => $info->translation,
                'album' => $info->album,
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }

    function  Save()
    {
        $save_dir = $this->ROOT_SAVE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        //save
        $savefilepath = $save_dir . "/idiom_db_list.json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

        $count = count($this->listItem);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listItem),
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }
}

$p = new CreateIdiomDB();
$p->DoCreate();
echo 'done<br>';
