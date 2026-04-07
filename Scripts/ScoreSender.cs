using Godot;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web; // 为了 Uri.EscapeDataString

public partial class ScoreSender : Node
{
    // 在编辑器中设置，不要硬编码
    [Export] private string privateKey = "QBYmoAxc3EqRPZuh1X2QlQq-BQ35nN60uWEF48o_unww";

    private static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    // 单例实例（自动加载后可通过类名访问）
    public static ScoreSender Instance { get; private set; }

    // 在 Godot 中调用此方法添加分数
    public async Task<bool> AddScore(string playerName, int score)
    {
        // 对名字进行 URL 编码
        string encodedName = HttpUtility.UrlEncode(playerName);
        string url = $"http://dreamlo.com/lb/{privateKey}/add/{encodedName}/{score}";

        try
        {
            string response = await client.GetStringAsync(url);
            return response.Trim() == "OK";
        }
        // 文档2（ScoreSender.cs）的AddScore方法中修改catch块
        catch (Exception e)
        {
            GD.PrintErr($"添加分数失败: {e.Message}");
            GD.PrintErr($"请求URL: {url}");  // 打印请求的URL
            return false;
        }
    }
    /// <summary>
    /// 将旧名字的分数迁移到新名字（分数不变）
    /// </summary>
    public async Task<bool> ChangeName(string playerNameBefore, string playerNameAfter, int score)
    {
        // 1. 对名字进行 URL 编码
        string oldEncoded = HttpUtility.UrlEncode(playerNameBefore);
        string newEncoded = HttpUtility.UrlEncode(playerNameAfter);

        // 2. 用新名字添加相同分数
        string addUrl = $"http://dreamlo.com/lb/{privateKey}/add/{newEncoded}/{score}";

        // 3. 删除旧名字的记录
        string deleteUrl = $"http://dreamlo.com/lb/{privateKey}/delete/{oldEncoded}";

        try
        {
            // 先添加新记录
            string addResponse = await client.GetStringAsync(addUrl);
            if (addResponse.Trim() != "OK")
            {
                GD.PrintErr("添加新名字分数失败");
                return false;
            }

            // 再删除旧记录
            string deleteResponse = await client.GetStringAsync(deleteUrl);
            if (deleteResponse.Trim() != "OK")
            {
                GD.PrintErr("删除旧名字记录失败");
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            GD.PrintErr($"改名操作失败: {e.Message}");
            GD.PrintErr($"请求URL: {deleteUrl}");  // 打印请求的URL
            return false;
        }
    }

    // 示例：在 _Ready 中测试
    public override async void _Ready()
    {
        Instance = this;
    }
}