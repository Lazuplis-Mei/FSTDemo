using System.Text.RegularExpressions;

namespace FstDemo;

/// <summary>
/// FstDict Demo (just Add and Search)
/// </summary>
public class FstDict
{

    /// <summary>
    /// TreeNode of char
    /// </summary>
    /// <param name="Value">character of word</param>
    private record Node(char Value)
    {
        /// <summary>
        /// child nodes
        /// </summary>
        public List<Node> Children { get; } = new();
        /// <summary>
        /// is end of a word
        /// </summary>
        public bool IsEnd { get; set; }
        /// <summary>
        /// try get specific child node
        /// </summary>
        /// <param name="value">character of word</param>
        /// <returns></returns>
        public Node? GetChildNode(char value)
        {
            return Children.Find(n => n.Value == value);
        }
    }

    /// <summary>
    /// root nodes
    /// </summary>
    private readonly List<Node> rootList = new();

    /// <summary>
    /// ensure <paramref name="word"/> is in correct format : [a-z]+
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static string EnsureWord(string word)
    {
        word = word.Trim().ToLower();
        if (!Regex.IsMatch(word, "[a-z]+"))
        {
            throw new ArgumentException($"invaild format of word", nameof(word));
        }
        return word;
    }

    /// <summary>
    /// try get specific root node
    /// </summary>
    /// <param name="value">first character of word</param>
    /// <returns></returns>
    private Node? GetRootNode(char value)
    {
        return rootList.Find(n => n.Value == value);
    }

    /// <summary>
    /// add a word to FstDict
    /// </summary>
    /// <param name="word"></param>
    public void AddWord(string word)
    {
        word = EnsureWord(word);

        var node = GetRootNode(word[0]);

        if (node is null)
        {
            //add a new root node (with child nodes)
            node = new Node(word[0]);
            rootList.Add(node);
            foreach (char @char in word[1..])
            {
                var temp = new Node(@char);
                node.Children.Add(temp);
                node = temp;
            }
        }
        else
        {

            int i = 0;
            //like move the pointer to correct index
            while (node?.Value == word[i])
            {
                if (++i >= word.Length)
                    break;
                var temp = node.GetChildNode(word[i]);
                if (temp is null)
                    break;
                node = temp;
            }

            //create new node connected to main
            while (i < word.Length)
            {
                var temp = new Node(word[i]);
                node?.Children.Add(temp);
                node = temp;
                i++;
            }
        }

        //set IsEnd flag
        if (node is not null)
        {
            node.IsEnd = true;
        }
    }

    /// <summary>
    /// search words starts with <paramref name="str"/>
    /// </summary>
    /// <param name="str">start str to search</param>
    /// <returns></returns>
    public List<string> SearchWord(string str)
    {
        str = EnsureWord(str);

        var result = new List<string>();
        var word = str;//prefix string

        var node = GetRootNode(str[0]);

        if (node is not null)
        {
            //like move the pointer to correct index
            foreach (char @char in str[1..])
            {
                node = node.GetChildNode(@char);
                //failed to find
                if (node is null)
                    return result;
            }

            //exactly match
            if (node?.IsEnd == true)
            {
                result.Add(word);
            }

            //local function which use *recursion* to search
            void InternalRecursionFunction(string word, Node? node)
            {
                if (node is not null)
                {
                    foreach (var child in node.Children)
                    {
                        var temp = word;
                        temp += child.Value;
                        if (child.IsEnd)
                        {
                            result.Add(temp);
                        }
                        InternalRecursionFunction(temp, child);
                    }
                }
            }

            InternalRecursionFunction(word, node);
        }

        return result;
    }
}
