namespace TRTextProcessingClassLibrary.Tool
{
    internal class CalculationClass
    {
        /// <summary>
        /// 减少误差的加法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Plus(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 + d2);
        }

        /// <summary>
        /// 减少误差的减法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Minus(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 - d2);
        }

        /// <summary>
        /// 减少误差的乘法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Multi(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 * d2);
        }

        /// <summary>
        /// 减少误差的除法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Divide(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 / d2);
        }
    }
}
