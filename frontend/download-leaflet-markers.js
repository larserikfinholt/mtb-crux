// Script to download Leaflet marker icons to public directory
import { createWriteStream, existsSync, mkdirSync, unlink } from 'fs';
import { get } from 'https';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

// Get the directory path
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const publicDir = join(__dirname, 'public');

// Create public directory if it doesn't exist
if (!existsSync(publicDir)) {
  mkdirSync(publicDir);
}

// List of files to download
const files = [
  {
    url: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
    dest: join(publicDir, 'marker-icon.png')
  },
  {
    url: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
    dest: join(publicDir, 'marker-icon-2x.png')
  },
  {
    url: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
    dest: join(publicDir, 'marker-shadow.png')
  }
];

// Function to download file
function downloadFile(url, dest) {
  return new Promise((resolve, reject) => {
    const file = createWriteStream(dest);
    get(url, (response) => {
      response.pipe(file);
      file.on('finish', () => {
        file.close();
        console.log(`Downloaded: ${dest}`);
        resolve();
      });
    }).on('error', (err) => {
      unlink(dest, () => {}); // Delete the file on error
      console.error(`Error downloading ${url}: ${err.message}`);
      reject(err);
    });
  });
}

// Download all files
async function downloadAll() {
  try {
    for (const file of files) {
      await downloadFile(file.url, file.dest);
    }
    console.log('All Leaflet marker icons downloaded successfully!');
  } catch (error) {
    console.error('Error downloading Leaflet marker icons:', error);
  }
}

downloadAll(); 