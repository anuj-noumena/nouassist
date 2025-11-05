(() => {
    let txtContent = '';
    const onClick = (event) => {
        event.preventDefault();
        event.stopPropagation();
        alert();
        navigator.clipboard.writeText(txtContent);
        document.removeEventListener("click", onClick);
        console.log("Text copied to clipboard.");
        console.log("Text:", txtContent);
        txtContent = ''; // Clear data after copying
    }

    document.addEventListener("click", onClick);

    const processData = (jsonData) => {
        if (!jsonData || !jsonData.chat_messages) {
            console.error("Invalid data format: 'chat_messages' property is missing.");
            return null;
        }

        let textContent = '';

        for (const message of jsonData.chat_messages) {
            if (message.sender === 'assistant' && Array.isArray(message.content)) {
                for (const content of message.content) {
                    if (content.type === 'text') {
                        textContent += content.text + "\n";
                    } else if (content.type === 'tool_use' && 'input' in content) {
                        if ('content' in content.input) {
                            const title = content.input.title || '';
                            textContent += `<antArtifact title="${title}">\n${content.input.content}\n</antArtifact>\n`;
                        } else if (content.input.command === 'update' && 'old_str' in content.input && 'new_str' in content.input) {
                            // Use a RegExp with the 'g' flag to replace all occurrences
                            const oldStrRegex = new RegExp(content.input.old_str, 'g');
                            textContent = textContent.replace(oldStrRegex, content.input.new_str);
                        }
                    }
                }
            }
        }

        return textContent;
    }

    const getData = async () => {
        const chatId = window.location.pathname.split("/").pop();
        const response = await fetch(
            `https://claude.ai/api/organizations/fab05e05-db4c-4d79-8fa0-640cd2946b43/chat_conversations/${chatId}?tree=True&rendering_mode=messages&render_all_tools=true`
        );
        return await response.json();
    };

    const start = async () => {
        const jsonData = await getData();
        txtContent = processData(jsonData);
        alert("Data processing complete. Click anywhere to copy the data to clipboard.");
    }
    start();
})();
