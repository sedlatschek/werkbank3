﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.operations;

namespace tests.operations
{
    [TestClass]
    public class CopyTest
    {
        [TestMethod]
        public void PerformWorksForDir()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            File.WriteAllText(Path.Combine(source, "hi.txt"), "hi");
            DirectoryInfo subDir = Directory.CreateDirectory(Path.Combine(source, "subdir"));
            File.WriteAllText(Path.Combine(subDir.FullName, "ho.txt"), "ho");
            string dest = Util.GetTempPath();

            Copy.Perform(source, dest);

            string hi = Path.Combine(dest, "hi.txt");
            string ho = Path.Combine(dest, "subdir", "ho.txt");

            Assert.IsTrue(File.Exists(hi));
            Assert.AreEqual(File.ReadAllText(hi), "hi");
            Assert.IsTrue(File.Exists(ho));
            Assert.AreEqual(File.ReadAllText(ho), "ho");
        }

        [TestMethod]
        public void PerformWorksForFile()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            Copy.Perform(source, dest);

            Assert.IsTrue(File.Exists(dest));
            Assert.AreEqual("hi", File.ReadAllText(dest));
        }

        [TestMethod]
        public void PerformWorksWithIgnoreListForSingleFileInRootDirectory()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);

            string hiPath = Path.Combine(source, "hi.txt");

            File.WriteAllText(hiPath, "hi");
            File.WriteAllText(Path.Combine(source, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(source, "ho.txt"), "ho");

            string dest = Util.GetTempPath();

            MatchingList ignoreList = new(new List<string>()
            {
                hiPath
            });

            Copy.Perform(source, dest, ignoreList);

            Assert.IsFalse(File.Exists(Path.Combine(dest, "hi.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(dest, "ha.txt")));
            Assert.IsTrue(File.Exists(Path.Combine(dest, "ho.txt")));
        }

        [TestMethod]
        public void PerformWorksWithIgnoreListForEntireDirectory()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);

            string dest = Util.GetTempPath();

            MatchingList ignoreList = new(new List<string>()
            {
                source
            });

            Copy.Perform(source, dest, ignoreList);

            Assert.IsFalse(Directory.Exists(dest));
        }

        [TestMethod]
        public void PerformWorksWithIgnoreListWithNestedDirectoriesAndFiles()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();

            string notIgnoredDirSourcePath = Path.Combine(source, "not_ignored");
            string ignoredDirSourcePath = Path.Combine(source, "ignored");
            string ignoredFileInNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "ignored.txt");
            string notIgnoredFile1InNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "not_ignored1.txt");
            string notIgnoredFile2InNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "not_ignored2.txt");
            string ignoredFileInIgnoredDirSourcePath = Path.Combine(ignoredDirSourcePath, "ignored_anyways.txt");

            string notIgnoredDirDestPath = Path.Combine(dest, "not_ignored");
            string ignoredDirDestPath = Path.Combine(dest, "ignored");
            string ignoredFileInNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "ignored.txt");
            string notIgnoredFile1InNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "not_ignored1.txt");
            string notIgnoredFile2InNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "not_ignored2.txt");
            string ignoredFileInIgnoredDirDestPath = Path.Combine(ignoredDirDestPath, "ignored_anyways.txt");

            Directory.CreateDirectory(notIgnoredDirSourcePath);
            Directory.CreateDirectory(ignoredDirSourcePath);

            File.WriteAllText(ignoredFileInNotIgnoredDirSourcePath, "I should be ignored");
            File.WriteAllText(notIgnoredFile1InNotIgnoredDirSourcePath, "I should be copied");
            File.WriteAllText(notIgnoredFile2InNotIgnoredDirSourcePath, "I should be copied");
            File.WriteAllText(ignoredFileInIgnoredDirSourcePath, "I should be ignored");

            MatchingList ignoreList = new(new List<string>()
            {
                ignoredDirSourcePath,
                ignoredFileInNotIgnoredDirSourcePath
            });

            Copy.Perform(source, dest, ignoreList);

            Assert.IsTrue(Directory.Exists(dest));
            Assert.IsTrue(Directory.Exists(notIgnoredDirSourcePath));
            Assert.IsFalse(Directory.Exists(ignoredDirDestPath));
            Assert.IsFalse(File.Exists(ignoredFileInNotIgnoredDirDestPath));
            Assert.IsTrue(File.Exists(notIgnoredFile1InNotIgnoredDirDestPath));
            Assert.IsTrue(File.Exists(notIgnoredFile2InNotIgnoredDirDestPath));
            Assert.IsFalse(File.Exists(ignoredFileInIgnoredDirDestPath));
        }

        [TestMethod]
        public void PerformWorksWithIgnorePattern()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();

            string sourceNodeModulesDir = Directory.CreateDirectory(Path.Combine(source, "node_modules")).FullName;
            string destNodeModulesDir = Path.Combine(dest, "node_modules");
            string sourceNestedDir = Path.Combine(source, "hi");
            string destNestedDir = Path.Combine(dest, "hi");
            string sourceNestedNodeModulesDir = Directory.CreateDirectory(Path.Combine(sourceNestedDir, "node_modules")).FullName;
            string destNestedNodeModulesDir = Path.Combine(destNestedDir, "node_modules");
            string sourceRootLevelFile = Path.Combine(source, "hi.txt");
            string destRootLevelFile = Path.Combine(dest, "hi.txt");
            string sourceNodeModulesFile = Path.Combine(sourceNodeModulesDir, "ho.txt");
            string destNodeModulesFile = Path.Combine(destNodeModulesDir, "ho.txt");
            string sourceNestedFile = Path.Combine(sourceNestedDir, "hu.txt");
            string destNestedFile = Path.Combine(destNestedDir, "hu.txt");
            string sourceNestedNodeModulesFile = Path.Combine(sourceNestedNodeModulesDir, "ha.txt");
            string destNestedNodeModulesFile = Path.Combine(destNestedNodeModulesDir, "ha.txt");

            File.WriteAllText(sourceRootLevelFile, "hi");
            File.WriteAllText(sourceNodeModulesFile, "ho");
            File.WriteAllText(sourceNestedNodeModulesFile, "ha");
            File.WriteAllText(sourceNestedFile, "ha");

            MatchingList ignoreList = new(null, new List<string>()
            {
                @".*\\node_modules\\{0,1}.*"
            });

            Copy.Perform(source, dest, ignoreList);

            Assert.IsTrue(Directory.Exists(dest));
            Assert.IsTrue(File.Exists(destRootLevelFile));
            Assert.IsTrue(File.Exists(destNestedFile));
            Assert.IsFalse(Directory.Exists(destNodeModulesDir));
            Assert.IsFalse(File.Exists(destNodeModulesFile));
            Assert.IsFalse(Directory.Exists(destNestedNodeModulesDir));
            Assert.IsFalse(File.Exists(destNestedNodeModulesFile));
        }

        [TestMethod]
        public void VerifyWorks()
        {
            string path1 = Util.GetTempPath();
            string path2 = Util.GetTempPath();

            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);

            File.WriteAllText(Path.Combine(path1, "hi.txt"), "hi");
            File.WriteAllText(Path.Combine(path2, "hi.txt"), "hi");

            DirectoryInfo subDir1 = Directory.CreateDirectory(Path.Combine(path1, "subdir"));
            DirectoryInfo subDir2 = Directory.CreateDirectory(Path.Combine(path2, "subdir"));

            File.WriteAllText(Path.Combine(subDir1.FullName, "ho.txt"), "ho");
            File.WriteAllText(Path.Combine(subDir2.FullName, "ho.txt"), "ho");

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.WriteAllText(Path.Combine(subDir2.FullName, "ho.txt"), "I changed this");

            Assert.IsFalse(Copy.Verify(path1, path2));

            File.WriteAllText(Path.Combine(subDir1.FullName, "ho.txt"), "I changed this");

            Assert.IsTrue(Copy.Verify(path1, path2));

            Directory.CreateDirectory(Path.Combine(path1, "somedir"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            Directory.Delete(Path.Combine(path1, "somedir"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            Directory.CreateDirectory(Path.Combine(path2, "somedir"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            Directory.Delete(Path.Combine(path2, "somedir"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(path1, "hi.txt"));

            Assert.IsFalse(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(path2, "hi.txt"));

            Assert.IsTrue(Copy.Verify(path1, path2));

            File.Delete(Path.Combine(subDir2.FullName, "ho.txt"));

            Assert.IsFalse(Copy.Verify(path1, path2));
        }

        [TestMethod]
        public void VerifyWorksForFile()
        {
            string source = Util.GetTempPath() + ".txt";
            File.WriteAllText(source, "hi");
            string dest = Util.GetTempPath() + ".txt";

            Copy.Perform(source, dest);

            Assert.IsTrue(Copy.Verify(source, dest));

            File.Delete(dest);
            Assert.IsFalse(Copy.Verify(source, dest));

            File.WriteAllText(dest, "hi");
            Assert.IsTrue(Copy.Verify(source, dest));

            File.WriteAllText(dest, "ho");
            Assert.IsFalse(Copy.Verify(source, dest));
        }

        [TestMethod]
        public void VerifyWorksWithIgnoreListForSingleFileInRootDirectory()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();
            Directory.CreateDirectory(dest);

            string hiPath = Path.Combine(source, "hi.txt");

            File.WriteAllText(hiPath, "hi");
            File.WriteAllText(Path.Combine(source, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(source, "ho.txt"), "ho");

            File.WriteAllText(Path.Combine(dest, "ha.txt"), "ha");
            File.WriteAllText(Path.Combine(dest, "ho.txt"), "ho");

            MatchingList ignoreList = new(new List<string>()
            {
                hiPath
            });

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            File.WriteAllText(Path.Combine(dest, "hi.txt"), "hi");

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            File.Delete(Path.Combine(dest, "hi.txt"));

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            File.WriteAllText(Path.Combine(dest, "ha.txt"), "hx");

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            File.WriteAllText(Path.Combine(dest, "ha.txt"), "ha");

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));
        }

        [TestMethod]
        public void VerifyWorksWithIgnoreListForEntireDirectory()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();

            MatchingList ignoreList = new(new List<string>()
            {
                source
            });

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(dest);

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));
        }

        [TestMethod]
        public void VerifyWorksWithIgnoreListWithNestedDirectoriesAndFiles()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();

            string notIgnoredDirSourcePath = Path.Combine(source, "not_ignored");
            string ignoredDirSourcePath = Path.Combine(source, "ignored");
            string ignoredFileInNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "ignored.txt");
            string notIgnoredFile1InNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "not_ignored1.txt");
            string notIgnoredFile2InNotIgnoredDirSourcePath = Path.Combine(notIgnoredDirSourcePath, "not_ignored2.txt");
            string ignoredFileInIgnoredDirSourcePath = Path.Combine(ignoredDirSourcePath, "ignored_anyways.txt");

            string notIgnoredDirDestPath = Path.Combine(dest, "not_ignored");
            string ignoredDirDestPath = Path.Combine(dest, "ignored");
            string ignoredFileInNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "ignored.txt");
            string notIgnoredFile1InNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "not_ignored1.txt");
            string notIgnoredFile2InNotIgnoredDirDestPath = Path.Combine(notIgnoredDirDestPath, "not_ignored2.txt");
            string ignoredFileInIgnoredDirDestPath = Path.Combine(ignoredDirDestPath, "ignored_anyways.txt");

            Directory.CreateDirectory(notIgnoredDirSourcePath);
            Directory.CreateDirectory(ignoredDirSourcePath);
            File.WriteAllText(ignoredFileInNotIgnoredDirSourcePath, "I should be ignored");
            File.WriteAllText(notIgnoredFile1InNotIgnoredDirSourcePath, "I should be copied");
            File.WriteAllText(notIgnoredFile2InNotIgnoredDirSourcePath, "I should be copied");
            File.WriteAllText(ignoredFileInIgnoredDirSourcePath, "I should be ignored");

            Directory.CreateDirectory(notIgnoredDirDestPath);
            File.WriteAllText(notIgnoredFile1InNotIgnoredDirDestPath, "I should be copied");
            File.WriteAllText(notIgnoredFile2InNotIgnoredDirDestPath, "I should be copied");

            MatchingList ignoreList = new(new List<string>()
            {
                ignoredDirSourcePath,
                ignoredFileInNotIgnoredDirSourcePath
            });

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(ignoredDirDestPath);

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            Directory.Delete(ignoredDirDestPath);

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(ignoredDirDestPath);
            File.WriteAllText(ignoredFileInIgnoredDirDestPath, "hi");

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            Directory.Delete(ignoredDirDestPath, true);

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(notIgnoredDirSourcePath);
            File.WriteAllText(ignoredFileInNotIgnoredDirDestPath, "hi");

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));
        }

        [TestMethod]
        public void VerifyWorksWithIgnorePattern()
        {
            string source = Util.GetTempPath();
            Directory.CreateDirectory(source);
            string dest = Util.GetTempPath();

            string sourceNodeModulesDir = Directory.CreateDirectory(Path.Combine(source, "node_modules")).FullName;
            string destNodeModulesDir = Path.Combine(dest, "node_modules");
            string sourceNestedDir = Path.Combine(source, "hi");
            string destNestedDir = Path.Combine(dest, "hi");
            string sourceNestedNodeModulesDir = Directory.CreateDirectory(Path.Combine(sourceNestedDir, "node_modules")).FullName;
            string destNestedNodeModulesDir = Path.Combine(destNestedDir, "node_modules");
            string sourceRootLevelFile = Path.Combine(source, "hi.txt");
            string destRootLevelFile = Path.Combine(dest, "hi.txt");
            string sourceNodeModulesFile = Path.Combine(sourceNodeModulesDir, "ho.txt");
            string sourceNestedFile = Path.Combine(sourceNestedDir, "hu.txt");
            string destNestedFile = Path.Combine(destNestedDir, "hu.txt");
            string sourceNestedNodeModulesFile = Path.Combine(sourceNestedNodeModulesDir, "ha.txt");
            string destNestedNodeModulesFile = Path.Combine(destNestedNodeModulesDir, "ha.txt");

            File.WriteAllText(sourceRootLevelFile, "hi");
            File.WriteAllText(sourceNodeModulesFile, "ho");
            File.WriteAllText(sourceNestedNodeModulesFile, "ha");
            File.WriteAllText(sourceNestedFile, "ha");

            MatchingList ignoreList = new(null, new List<string>()
            {
                @".*\\node_modules\\{0,1}.*"
            });

            Directory.CreateDirectory(dest);
            File.WriteAllText(destRootLevelFile, "hi");
            Directory.CreateDirectory(destNestedDir);
            File.WriteAllText(destNestedFile, "ha");

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(destNodeModulesDir);

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            Directory.Delete(destNodeModulesDir);

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));

            Directory.CreateDirectory(destNestedNodeModulesDir);

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            File.WriteAllText(destNestedNodeModulesFile, "ha");

            Assert.IsFalse(Copy.Verify(source, dest, ignoreList));

            Directory.Delete(destNestedNodeModulesDir, true);

            Assert.IsTrue(Copy.Verify(source, dest, ignoreList));
        }
    }
}
