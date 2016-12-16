namespace Monitor.Web.Response
{
    /// <summary>
    /// 管理员操作提示
    /// </summary>
    public class CodeStruct
    {
        /// <summary>
        /// 保存成功
        /// </summary>
        public static string SaveSuccess = "保存成功";

        /// <summary>
        /// 保存失败
        /// </summary>
        public static string SaveFail = "保存失败";

        /// <summary>
        /// 添加成功
        /// </summary>
        public static string AddSuccess = "添加成功";

        /// <summary>
        /// 添加失败
        /// </summary>
        public static string AddFail = "添加失败";

        /// <summary>
        /// 删除成功
        /// </summary>
        public static string DelSuccess = "删除成功";

        /// <summary>
        /// 删除失败
        /// </summary>
        public static string DelFail = "删除失败";

        /// <summary>
        /// 上传成功
        /// </summary>
        public static string UploadSuccess = "上传成功";

        /// <summary>
        /// 上传失败
        /// </summary>
        public static string UploadFail = "上传失败";

        /// <summary>
        /// 修改成功
        /// </summary>
        public static string UpdateSuccess = "修改成功";

        /// <summary>
        /// 修改失败
        /// </summary>
        public static string UpdateFail = "修改失败";

        /// <summary>
        /// 参数错误
        /// </summary>
        public static string ParameterError = "参数错误";

        /// <summary>
        /// 操作成功
        /// </summary>
        public static string OperatingSuccess = "操作成功";

        /// <summary>
        /// 数据已经存在
        /// </summary>
        public static string BusinessException = "业务异常";
    }

    public enum CodeEnmStruct
    {
        登录成功 = 10000,
        登录超时 = 10001,
        用户名错误 = 10002,
        密码错误 = 10003,
        验证码错误 = 10004,
        权限不够 = 10005,
        成功 = 10006,
        失败 = 10007,
        参数错误 = 10008,
        无数据 = 10009
    }
}
