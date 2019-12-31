/*********************************************************************** 
 * 项目名称 :  Fits.Framework.Cache  
 * 项目描述 :      
 * 类 名 称 :  MultiFileWatcher
 * 说    明 :      
 * 作    者 :  fangwenhan 
 * 创建时间 :  2014/11/18 15:12:18 
 * 更新时间 :  2014/11/18 15:12:18  
************************************************************************ 
 * Copyright @   2014. All rights reserved. 
************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;


namespace Peanut.Infrastructure.Cache.MemCached
{
    // MultiFileWatcher
    // 
    // 目的：
    //     监控缓存配置文件的变化并引发一个事件。
    // 
    // 使用规范：
    //     略   
    internal class MultiFileWatcher : IDisposable
    {
        private readonly List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        /// <summary>
        /// 引发事件
        /// </summary>
        public event EventHandler OnChange;

        public void Dispose()
        {
            StopWatching();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Stops the watching.
        /// </summary>
        public void StopWatching()
        {
            lock (this)
            {
                foreach (FileSystemWatcher watcher in watchers)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Dispose();
                }

                watchers.Clear();
            }
        }

        /// <summary>
        /// Watches the specified files for changes.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        public void Watch(IEnumerable<string> fileNames)
        {
            if (fileNames == null)
            {
                return;
            }

            foreach (string s in fileNames)
            {
                Watch(s);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Watcher is released in Dispose()")]
        internal void Watch(string fileName)
        {
            var watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(fileName),
                Filter = Path.GetFileName(fileName),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime 
                | NotifyFilters.Size | NotifyFilters.Security | NotifyFilters.Attributes
            };

            watcher.Created += OnWatcherChanged;
            watcher.Changed += OnWatcherChanged;
            watcher.Deleted += OnWatcherChanged;
            watcher.EnableRaisingEvents = true;

            lock (this)
            {
                watchers.Add(watcher);
            }
        }

        private void OnWatcherChanged(object source, FileSystemEventArgs e)
        {
            lock (this)
            {
                if (OnChange != null)
                    OnChange(source, e);
            }
        }
    }
}



