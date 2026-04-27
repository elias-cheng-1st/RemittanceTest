using RemittanceTest.Models;

namespace RemittanceTest.Services
{
    public class RemittanceService : IRemittanceService
    {
        // 狀態常數定義
        private const int STATUS_PROCESSING = 0;        // 處理中
        private const int STATUS_IN_TRANSACTION = 1;    // 交易中
        private const int STATUS_COMPLETED = 2;         // 已完成
        private const int STATUS_CANCELLED = 9;         // 已取消

        // 模擬資料庫 (靜態變數確保跨 Request 資料一致)
        private static readonly List<Remittance> _db = new()
        {
            new Remittance { Id = 1, AccountName = "測試企業A", Amount = 50000, Status = 0 },
            new Remittance { Id = 2, AccountName = "測試企業B", Amount = 12000, Status = 1 }, // 不可取消
            new Remittance { Id = 3, AccountName = "測試企業C", Amount = 30000, Status = 0 }
        };

        // 提示：如何確保多執行緒下的資料安全？
        private static readonly object _lockObj = new object();

        public (bool IsSuccess, string Message) CancelRemittance(int id)
        {
            lock (_lockObj)
            {
                var remittance = _db.FirstOrDefault(r => r.Id == id);

                if (remittance == null)
                {
                    return (false, "找不到該筆匯款");
                }

                switch (remittance.Status)
                {
                    case STATUS_PROCESSING: // 處理中，可取消
                        remittance.Status = STATUS_CANCELLED; // 更新狀態為已取消
                        return (true, "取消成功");

                    case STATUS_IN_TRANSACTION: // 交易中，不可取消
                        return (false, "該筆匯款已完成，不可取消");

                    case STATUS_COMPLETED: // 已完成，不可取消
                        return (false, "該筆匯款已經完成，不可取消");

                    case STATUS_CANCELLED: // 已取消
                        return (false, "該筆匯款已經取消");

                    default:
                        return (false, "未知的匯款狀態");
                }
            }
        }
    }
}