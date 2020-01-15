`#include <iostream>
#include <string> //Special stuff for string variables
#include <queue> //this has our priority queue in it. We'll need it to sort out our nodes
#include <unordered_map> //This has our "Hashtable" in it as C# would call it.
#include <fstream> //Manages our files
#include <vector>
#include <bitset>

using namespace std;
//a node or leaf on our tree. Holds the character, it's frequency, or a pointer to two other nodes.
struct Node {
char character; //The character in a string
int frequency; //How often the character occurs in the string.
Node* left; //Point to the node to the left. A '0' value when we start iterating through the tree.
Node* right; //Points to the node to the right. A '1' value when we start iterating through the tree.
};

//A method to construct a Node for our tree and return a pointer to it.
Node* createNode(char c, int f, Node* l, Node* r) {
Node* node = new Node();
node->character = c;
node->frequency = f;
node->left = l;
node->right = r;

    return node;

}

//A struct to hold the comparison information for our priority queue.
struct compare {
bool operator()(Node* l, Node* r) {
return l->frequency > r->frequency; //Low frequency = high priority.
}
};
class ConsoleInterface {
public:
void initializeFiles() {
ofstream os;
os.open("huffman.dat", ios::binary | ios::out); //binary file
os.close();
os.open("input.txt", ios::in);
os.close();
os.open("output.txt");
os.close();
}
bool inputFromFile(string& input) {
input;
ifstream in;
in.open("input.txt");
if (!in.fail()) {
cout << "input.txt has been successfully read" << endl;
input.assign((istreambuf_iterator<char>(in)), (istreambuf_iterator<char>()));
in.close();
if (input.size() < 1) {
cout << "No text found in file input.txt. Please review the file and try again." << endl;
return false;
}
return true;
}
else {
cout << "File input.txt failed to open" << endl;
in.close();
return false;
}
}
void options() {
cout << "Hello! Please select what you would like to do!" << endl;
cout << "1) for compression (Compresses what's in the \"input.txt\" file)" << endl;
cout << "2) for decompression (prints results in the \"output.txt\" file)" << endl;
cout << "3) to wipe the current data (wipes all files)" << endl;
cout << "4) to quit" << endl;
}
void wipeData() {
remove("huffman.dat");
remove("output.txt");
ifstream in;
in.open("output.txt");
in.close();
cout << "Data has been wiped!" << endl;
}

};
class Writer {
public:
//Traverse the tree and encode based on movement. '0' is left, '1' is right.
void saveLeafNodes(priority_queue<Node*, vector<Node*>, compare>& huffmanTree) {
Node* node;
ofstream outStream;
outStream.open("temp.dat", ios::out | ios::binary | ios::app);
queue<Node*> temp;

    	//Write the node values to a temporary file. This allows us to access the size of the tree.
    	//This was the only way I could get it to work
    	while (huffmanTree.size() > 0) {
    		temp.push(huffmanTree.top());
    		node = huffmanTree.top();
    		outStream.write((char*)& node->character, sizeof(node->character));
    		outStream.write((char*)& node->frequency, sizeof(node->frequency));
    		huffmanTree.pop();
    	}

    	//Save the size of the Huffman Tree by accessing the temporary file's size.
    	outStream.seekp(0, ios::end);
    	int size = outStream.tellp();
    	outStream.seekp(0, ios::beg);

    	//Push the node values back into the tree container.
    	while (temp.size() > 0) {
    		huffmanTree.push(temp.front());
    		temp.pop();
    	}

    	//The temp file has done its job. Close it and delete it.
    	outStream.close();
    	remove("temp.dat");

    	//Now we write the size of the tree to the actual compressed binary file.
    	outStream.open("huffman.dat", ios::out | ios::binary | ios::app);
    	outStream.write((char*)& size, sizeof(size));

    	//Write the tree to the binary file
    	cout << "Character : Frequency" << endl;
    	while (huffmanTree.size() > 0) {
    		temp.push(huffmanTree.top());
    		node = huffmanTree.top();
    		outStream.write((char*)& node->character, sizeof(node->character));
    		outStream.write((char*)& node->frequency, sizeof(node->frequency));
    		cout << node->character << " : " << node->frequency << endl;
    		huffmanTree.pop();
    	}

    	while (temp.size() > 0) {
    		huffmanTree.push(temp.front());
    		temp.pop();
    	}
    	outStream.close();
    }
    void saveEncodedString(string encodedStr) {
    	ofstream outStream;
    	outStream.open("huffman.dat", ios::out | ios::binary | ios::app);
    	vector<bool> vb;
    	bitset<8> bits;

    	//Convert ones and zeros to binary values in a vector
    	for (char c : encodedStr) {
    		if (c == '1') {
    			vb.push_back(true);
    		}
    		else {
    			vb.push_back(false);
    		}
    	}

    	//Access the size of the vector and see how many more values we need to make a complete byte.
    	//Write that buffer size to the binary file before the encoded string. We'll access it later.
    	int buffer = vb.size() % 8;
    	cout << "Buffer: " << 8 - buffer << endl;
    	outStream.write((char*)& buffer, sizeof(int));

    	//Turn the vector into a set 8 bits by using the bitset data type.
    	int count = 0;
    	for (bool b : vb) {
    		bits[count] = b;
    		if (count == 7) {
    			count = 0;
    			outStream.write((char*)& bits, sizeof(bool));
    		}
    		else {
    			count++;
    		}
    	}

    	//If we didn't make a full byte on the last set, fill it in with zeros.
    	//We'll remove them later by accessing the buffer we wrote at the beginning of the string.
    	if (count < 7) {
    		for (int i = count; i < 8; i++) {
    			bits[i] = 0;
    			cout << "Adding buffer..." << endl;
    		}
    		outStream.write((char*)& bits, sizeof(bool));
    	}
    	outStream.close();
    }

};
class Encoder {
public:
void createHuffmanTree(priority_queue<Node*, vector<Node*>, compare>& huffmanTree)
{
//Continue to connect leafs to nodes until there is only one node left in the tree. This is our root node.
while (huffmanTree.size() != 1) {
Node* left = huffmanTree.top(); //create a pointer to the left node
huffmanTree.pop(); //pop out the left node
Node* right = huffmanTree.top(); //Create a pointer to the right node
huffmanTree.pop(); //pop out the right node

    		int sum = left->frequency + right->frequency;
    		huffmanTree.push(createNode('\0', sum, left, right));
    	}
    }
    void printHuffmanCodes(unordered_map<char, string>& huffmanCodes)
    {
    	for (auto pair : huffmanCodes) {
    		cout << pair.first << " : " << pair.second << endl;
    	}
    }
    void createCharacterFrequencyMap(string& text, unordered_map<char, int>& charFreq)
    {
    	for (char c : text) {
    		charFreq[c]++;
    	}
    }
    void orderLeafNodes(unordered_map<char, int>& charFreq, priority_queue<Node*, vector<Node*>, compare>& huffmanTree) {
    	//Create leaf nodes and push it onto the priority queue. This will be our tree.
    	for (auto pair : charFreq) {
    		huffmanTree.push(createNode(pair.first, pair.second, nullptr, nullptr));
    	}
    }
    void encodeString(Node* node, string str, unordered_map<char, string>& huffmanCode) {
    	if (node == nullptr) {
    		return;
    	}
    	if (!node->left && !node->right) {
    		huffmanCode[node->character] = str;
    	}

    	encodeString(node->left, str + '0', huffmanCode);
    	encodeString(node->right, str + '1', huffmanCode);
    }
    void createEncodedString(string& text, string& encodedStr, unordered_map<char, string>& huffmanCodes)
    {
    	for (char c : text) {
    		encodedStr += huffmanCodes[c];
    	}
    }

};
class Reader {
private:
void getNodesFromFile(priority_queue<Node*, vector<Node*>, compare>& huffmanTree, ifstream& inStream) {
Node* node = new Node();
int size;
inStream.seekg(0, ios::beg);
inStream.read((char*)& size, sizeof(int));
cout << "Character : Frequency" << endl;
while (inStream.tellg() < size) {
inStream.read((char*)& node->character, sizeof(node->character));
inStream.read((char*)& node->frequency, sizeof(node->frequency));
cout << node->character << " : " << node->frequency << endl;
huffmanTree.push(createNode(node->character, node->frequency, nullptr, nullptr));
}
cout << "Number of nodes: " << huffmanTree.size() << endl;
}
public:
Node* getRootFromFile(priority_queue<Node*, vector<Node*>, compare>& huffmanTree, ifstream& inStream) {
Encoder* encoder = new Encoder();
getNodesFromFile(huffmanTree, inStream);
encoder->createHuffmanTree(huffmanTree);
Node\* root = huffmanTree.top();
delete encoder;
return root;

    }
    string getEncodedStringFromFile(ifstream& inStream) {
    	vector<bool>vb;

    	//Save our current spot in the file stream.
    	//We'll need to start our iterating from here.
    	int cur = inStream.tellg();
    	inStream.seekg(0, ios::end);
    	int max = inStream.tellg();
    	inStream.seekg(cur, ios::beg);

    	//We'll need two bitsets.
    		//One that we read directly from the binary file
    		//And one that will reverse the bits into the original form.
    	bitset<8> bits;
    	bitset<8> bitsReversed;
    	string str = "";

    	//Start off by reading our buffer
    	int buffer;
    	inStream.read((char*)& buffer, sizeof(int));
    	buffer = 8 - buffer;
    	cout << "Buffer: " << buffer << endl;

    	//Now read in the rest of the file
    	while (inStream.tellg() < max) {
    		inStream.read((char*)& bits, sizeof(bool));
    		//cout << bits << endl;
    		for (int i = 0; i < 8; i++) {
    			bitsReversed[i] = bits[7 - i];
    		}
    		str += bitsReversed.to_string();
    	}
    	cout << "Done!" << endl;

    	//Shave off our buffer from the final product. This is the useless information.
    	while (buffer != 0) {
    		str.pop_back();
    		buffer--;
    		cout << "Removing buffer..." << endl;
    	}
    	return str;
    }

};

void compress(string text) {
cout << "Compressing..." << endl;
Writer* writer = new Writer();
Encoder* encoder = new Encoder();
unordered_map<char, int> charFreq;
priority_queue<Node*, vector<Node*>, compare> huffmanTree;
unordered_map<char, string>huffmanCodes;

    //Store character and its frequency
    encoder->createCharacterFrequencyMap(text, charFreq);

    //Create the Huffman Tree
    encoder->orderLeafNodes(charFreq, huffmanTree);
    writer->saveLeafNodes(huffmanTree);
    encoder->createHuffmanTree(huffmanTree);

    Node* root = huffmanTree.top();
    cout << huffmanTree.size() << endl;
    encoder->encodeString(root, "", huffmanCodes);

    huffmanTree = priority_queue<Node*, vector<Node*>, compare>();
    cout << "Huffman Codes are:" << endl;

    //print out huffman codes
    encoder->printHuffmanCodes(huffmanCodes);

    cout << "Original string was: " << text << endl;

    //print encoded string
    string encodedStr = "";
    encoder->createEncodedString(text, encodedStr, huffmanCodes);
    writer->saveEncodedString(encodedStr);
    cout << "Encoded string is: \n" << encodedStr << endl;
    cout << "Compression complete! Feel free to review the files to ensure your file has been compressed. The compressed file is huffman.dat" << endl;
    cout << endl;

}
//traverse the huffman tree and decode. If we hit a 0 in the encoded string, go left. If we hit a 1, go right.
//Once there are no more left or right paths, we have found a leaf node and can print out the character.
void decode(Node\* node, int& index, string str, string& decodeStr) {
if (node == nullptr) {
return;
}

    if (!node->left && !node->right) {
    	decodeStr += node->character;
    	return;
    }

    index++;

    if (str[index] == '0')
    	decode(node->left, index, str, decodeStr);
    else
    	decode(node->right, index, str, decodeStr);

}
void writeOutput(string& encodedStr, Node* root)
{
ofstream out;
out.open("output.txt", ios::out | ios::app);
int index = -1;
cout << "Decoded string is: \n" << endl;
string decodedString = "";
while (index < (int)encodedStr.size() - 2) {
decode(root, index, encodedStr, decodedString);
}
cout << decodedString << endl;
out << decodedString;
}
void decompress() {
Reader* reader = new Reader();
Node* root;
string encodedStr;
priority_queue<Node*, vector<Node*>, compare> huffmanTree;
cout << "Decompress" << endl;
ifstream inStream;
inStream.open("huffman.dat", ios::binary | ios::in);
root = reader->getRootFromFile(huffmanTree, inStream);
encodedStr = reader->getEncodedStringFromFile(inStream);
//Decode the encoded string
writeOutput(encodedStr, root);
delete reader;
}
int main() {
ConsoleInterface* consoleInterface = new ConsoleInterface();
consoleInterface->initializeFiles();
string stringToEncode;
int dec = 0;
do {
cout << endl;
consoleInterface->options();
cin >> dec;
if (dec == 1) {
if (consoleInterface->inputFromFile(stringToEncode)) {
compress(stringToEncode);
}
}
else if (dec == 2) {
ifstream is;
is.open("huffman.dat", ios::binary | ios::out);
is.seekg(0, ios::end);
int size = is.tellg();
is.close();
if (size < 1) {
cout << "No data found" << endl;
}
else {
decompress();
}
}
else if (dec == 3) {
consoleInterface->wipeData();
}
else if (dec == 4) {
break;
}
else {
cout << "Invalid option" << endl;
}
} while (true);
cout << "Goodbye!";
delete consoleInterface;
return 0;
}`
