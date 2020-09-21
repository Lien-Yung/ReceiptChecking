# ReceiptChecking
可輸入統一發票號碼及月份，來獲得是否中獎、中幾獎等資訊

# interface
* Reward => 對獎  
* Winning => 中獎資訊
```
public interface IChecking
{
    void Reward(string number);
    void Winning(string item);
}
```
