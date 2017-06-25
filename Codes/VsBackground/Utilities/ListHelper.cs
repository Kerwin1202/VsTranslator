using System.Collections;
using System.Collections.Generic;

namespace VsBackground.Utilities
{
    public class ListHelper : List<string>
    {
        private int _curIndex = 0;

        /// <summary>
        /// 向后移
        /// </summary>
        public void MoveNext()
        {
            _curIndex++;
            if (_curIndex >= Count)
            {
                _curIndex = 0;
            }
        }
        /// <summary>
        /// 获取当前
        /// </summary>
        /// <returns></returns>
        public string Current => Count > 0 ? this[_curIndex] : null;
    }
}