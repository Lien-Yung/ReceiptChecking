# ReceiptChecking

* 自製簡易C# 統一發票對獎
  * 可輸入統一發票號碼及月份，來獲得是否中獎、中幾獎等資訊
  
透過 URL 取得網頁中的內容 
```
 WebRequest myRequest = WebRequest.Create(url);
 
//Method選擇GET
 myRequest.Method = "GET";
 
//取得WebRequest的回覆
WebResponse myResponse = myRequest.GetResponse();

//Streamreader讀取回覆
StreamReader sr = new StreamReader(myResponse.GetResponseStream());

//將全文轉成string
string result = sr.ReadToEnd();

//關掉StreamReader
sr.Close();

//關掉WebResponse
myResponse.Close();
```
透過正規表達式找出個獎項的中獎號碼
```
Regex.Match(result, @"(?<=<td headers=""specialPrize"" class=""number""> )((?:\d)+)(?= </td>)", RegexOptions.IgnoreCase).ToString();
```
# Interface
定義一個 IChecking 介面 提供 Reward, Winning 功能
* Reward => 對獎  
* Winning => 中獎資訊
```
void Reward(string number);
void Winning(string item);

```

定義 Checking 並實現 IChecking實作 連接方式
```
public class Checking : IChecking
{
    public void Reward(string number)
    {
        //...
    }
    public void Winning(string item)
    {
        //...
    }
}

```

